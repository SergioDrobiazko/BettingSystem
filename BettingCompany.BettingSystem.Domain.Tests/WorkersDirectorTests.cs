using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BettingCompany.BettingSystem.Domain.Tests
{
    public class WorkersDirectorTests
    {
        [Fact]
        public void DelegateWork()
        {
            var workersDirector = new WorkersDirector(100, new WorkersFactory());

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

            workersDirector.DelegateBetCalculation(betTransition);
        }

        [Fact]
        public async Task DelegateWork_ThenCancel()
        {
            Mock<IWorker> mockWorker = new Mock<IWorker>();

            mockWorker.Setup(x => x.CalculateBetAsync(It.IsAny<BetTransition>(), It.IsAny<CancellationToken>()))
                .Returns(
                    async (BetTransition bt, CancellationToken ct) =>
                    {
                        await Task.Delay(50, ct);
                        return new BetCalculated(betTransition: null, betOutcome: BetOutcome.Won(250));
                    });

            Mock<IWorkersFactory> mockWokersFactory = new Mock<IWorkersFactory>();

            mockWokersFactory.Setup(x => x.CreateWorker())
                .Returns(mockWorker.Object);

            var workersDirector = new WorkersDirector(100, mockWokersFactory.Object);

            var betArrivedFirst = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.OPEN);
            var betArrivedSecond = new Bet(1, 100, 2.5, "Bob", "Lenox Luis vs Vitaly Klichko", "", "Klichko Wins", BetStatus.WINNER);

            var betTransition = new BetTransition(betArrivedFirst, betArrivedSecond);

            workersDirector.DelegateBetCalculation(betTransition);

            await Task.Delay(110);

            workersDirector.ShutDown();
        }
    }
}
