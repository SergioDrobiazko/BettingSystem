using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BettingCompany.BettingSystem.Domain.Tests
{
    public class BetAgregatorTests
    {
        [Fact]
        public void AddBet_AddTwoBets_TransitionFormed()
        {
            var betArrivedFirst = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.OPEN);
            var betArrivedSecond = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.WINNER);

            var betAgregator = new BetAgregator();

            BetTransition betTransition = default;

            betAgregator.BetTransitionFormed += (object sender, BetTransitionFormedEventArgs e) =>
            {
                betTransition = e.BetTransition;
            };

            betAgregator.AddBet(betArrivedFirst);
            betAgregator.AddBet(betArrivedSecond);

            Assert.Equal(100, betTransition.InitialBet.Amount);
            Assert.Equal(1, betTransition.InitialBet.Id);
            Assert.Equal(2.5, betTransition.InitialBet.Odds);

            Assert.Equal(100, betTransition.SecondaryBet.Amount);
            Assert.Equal(1, betTransition.SecondaryBet.Id);
            Assert.Equal(2.5, betTransition.SecondaryBet.Odds);
        }

    }
}

{
    "id": 1,
  "amount": 100,
  "odds":  2.5,
  "client": "Bob",
  "event": "Lenox Luis vs Vitaly Klichko",
  "market": "string",
  "selection": "Klichko Wins",
  "status": 0
}
