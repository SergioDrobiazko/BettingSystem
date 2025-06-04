using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BettingCompany.BettingSystem.Domain.Tests
{
    public class WorkersDirectorTests
    {
        [Fact]
        public void DelegateWork()
        {
            var workersDirector = new WorkersDirector(100);

            var betArrivedFirst = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.OPEN);
            var betArrivedSecond = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.WINNER);

            var betTransition = new BetTransition(betArrivedFirst, betArrivedSecond);

            BetCalculated calculatedBet = default;

            workersDirector.BetCalculated += (object sender, BetCalculatedEventArgs e) =>
            {
                calculatedBet = workersDirector.FetchCalculatedBet();

                Assert.Equal(BetOutcomeStatus.Won, calculatedBet.BetOutcome.Status);
                Assert.Equal(250, calculatedBet.BetOutcome.Amount);
            };

            workersDirector.DelegateWork(betTransition);
        }
    }
}
