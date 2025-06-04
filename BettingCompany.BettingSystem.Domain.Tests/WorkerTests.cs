using System;
using System.Threading.Tasks;
using Xunit;

namespace BettingCompany.BettingSystem.Domain.Tests
{
    public class WorkerTests
    {
        [Fact]
        public async Task CalculateBet_IncorrectTransitionStatus_MarkedForReview()
        {
            var worker = new Worker();

            var betArrivedFirst = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.WINNER);
            var betArrivedSecond = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.OPEN);

            var betTransition = new BetTransition(betArrivedFirst, betArrivedSecond);

            var betOutcomeTask = worker.CalculateBetAsync(betTransition);

            var betOutcome = await betOutcomeTask;

            Assert.True(betOutcome.IsMarkedForReview);
        }

        [Fact]
        public async Task CalculateBet_CorrectTransitionStatus_NotMarkedForReview()
        {
            var worker = new Worker();

            var betArrivedFirst = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.OPEN);
            var betArrivedSecond = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.WINNER);

            var betTransition = new BetTransition(betArrivedFirst, betArrivedSecond);

            var betOutcomeTask = worker.CalculateBetAsync(betTransition);

            var betOutcome = await betOutcomeTask;

            Assert.False(betOutcome.IsMarkedForReview);
        }

        [Fact]
        public async Task CalculateBet_ClientWin_CorrectProfit()
        {
            var worker = new Worker();

            var betArrivedFirst = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.OPEN);
            var betArrivedSecond = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.WINNER);

            var betTransition = new BetTransition(betArrivedFirst, betArrivedSecond);

            var betOutcomeTask = worker.CalculateBetAsync(betTransition);

            var betOutcome = await betOutcomeTask;

            Assert.Equal(betOutcome.BetOutcome.Status, BetOutcomeStatus.Won);

            Assert.Equal(250, betOutcome.BetOutcome.Amount);
        }

        [Fact]
        public async Task CalculateBet_ClientLose_CorrectLoss()
        {
            var worker = new Worker();

            var betArrivedFirst = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Lenox Wins", BetStatus.OPEN);
            var betArrivedSecond = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Lenox Wins", BetStatus.LOSER);

            var betTransition = new BetTransition(betArrivedFirst, betArrivedSecond);

            var betOutcomeTask = worker.CalculateBetAsync(betTransition);

            var betOutcome = await betOutcomeTask;

            Assert.Equal(betOutcome.BetOutcome.Status, BetOutcomeStatus.Lost);

            Assert.Equal(100, betOutcome.BetOutcome.Amount);
        }

        [Fact]
        public async Task CalculateBet_ClientVoid_CorrectOutcome()
        {
            var worker = new Worker();

            var betArrivedFirst = new Bet(2, 1000, 5.5, "Sandra", "Usain Bolt 100m in London Olympics 2012", "", "Bolt breaks the WR", BetStatus.OPEN);
            var betArrivedSecond = new Bet(2, 1000, 5.5, "Sandra", "Usain Bolt 100m in London Olympics 2012", "", "Bolt breaks the WR", BetStatus.VOID);

            var betTransition = new BetTransition(betArrivedFirst, betArrivedSecond);

            var betOutcomeTask = worker.CalculateBetAsync(betTransition);

            var betOutcome = await betOutcomeTask;

            Assert.Equal(betOutcome.BetOutcome.Status, BetOutcomeStatus.Void);

            Assert.Equal(1000, betOutcome.BetOutcome.Amount);
        }
    }
}
