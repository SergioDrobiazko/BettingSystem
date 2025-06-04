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

            var betOutcomeTask = worker.CalculateBet(betTransition);

            var betOutcome = await betOutcomeTask;

            Assert.True(betOutcome.IsMarkedForReview);
        }
    }
}
