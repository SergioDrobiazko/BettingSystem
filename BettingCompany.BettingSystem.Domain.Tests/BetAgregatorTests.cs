﻿using Xunit;

namespace BettingCompany.BettingSystem.Domain.Tests
{
    public class BetAgregatorTests
    {
        [Fact]
        public void AddBet_AddTwoBets_TransitionFormed()
        {
            var betArrivedFirst = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.OPEN);
            var betArrivedSecond = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.WINNER);

            var betAgregator = new BetAggregator();

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