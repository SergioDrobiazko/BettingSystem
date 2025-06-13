using Xunit;

namespace BettingCompany.BettingSystem.Domain.Tests
{
    public class BetTransitionTests
    {
        [Fact]
        public void IsValidStatusTransition_FromOpenToWinner_IsValid()
        {
            var transition = new BetTransition(
                new Bet(id: 1, amount: 420, odds: 4.08, client: "David", @event: "David vs Eve", market: "Correct Score", selection: "3:2", status: BetStatus.OPEN),
                new Bet(id: 1, amount: 420, odds: 4.08, client: "David", @event: "David vs Eve", market: "Correct Score", selection: "3:2", status: BetStatus.WINNER));

            Assert.True(transition.IsValidStatusTransition());
        }

        [Fact]
        public void IsValidStatusTransition_FromOpenToLoser_IsValid()
        {
            var transition = new BetTransition(
                new Bet(id: 1, amount: 420, odds: 4.08, client: "David", @event: "David vs Eve", market: "Correct Score", selection: "3:2", status: BetStatus.OPEN),
                new Bet(id: 1, amount: 420, odds: 4.08, client: "David", @event: "David vs Eve", market: "Correct Score", selection: "3:2", status: BetStatus.LOSER));

            Assert.True(transition.IsValidStatusTransition());
        }

        [Fact]
        public void IsValidStatusTransition_FromOpenToVoid_IsValid()
        {
            var transition = new BetTransition(
                new Bet(id: 1, amount: 420, odds: 4.08, client: "David", @event: "David vs Eve", market: "Correct Score", selection: "3:2", status: BetStatus.OPEN),
                new Bet(id: 1, amount: 420, odds: 4.08, client: "David", @event: "David vs Eve", market: "Correct Score", selection: "3:2", status: BetStatus.VOID));

            Assert.True(transition.IsValidStatusTransition());
        }

        [Fact]
        public void IsValidStatusTransition_FromVoidToOpen_IsNotValid()
        {
            var transition = new BetTransition(
                new Bet(id: 1, amount: 420, odds: 4.08, client: "David", @event: "David vs Eve", market: "Correct Score", selection: "3:2", status: BetStatus.VOID),
                new Bet(id: 1, amount: 420, odds: 4.08, client: "David", @event: "David vs Eve", market: "Correct Score", selection: "3:2", status: BetStatus.OPEN));

            Assert.False(transition.IsValidStatusTransition());
        }

        [Fact]
        public void IsValidStatusTransition_FromWinnerToOpen_IsNotValid()
        {
            var transition = new BetTransition(
                new Bet(id: 1, amount: 420, odds: 4.08, client: "David", @event: "David vs Eve", market: "Correct Score", selection: "3:2", status: BetStatus.WINNER),
                new Bet(id: 1, amount: 420, odds: 4.08, client: "David", @event: "David vs Eve", market: "Correct Score", selection: "3:2", status: BetStatus.OPEN));

            Assert.False(transition.IsValidStatusTransition());
        }

        [Fact]
        public void CalculateBetOutcome_DavidWins_CorrectAmountAndStatus()
        {
            var transition = new BetTransition(
                new Bet(id: 1, amount: 420, odds: 4.08, client: "David", @event: "David vs Eve", market: "Correct Score", selection: "3:2", status: BetStatus.OPEN),
                new Bet(id: 1, amount: 420, odds: 4.08, client: "David", @event: "David vs Eve", market: "Correct Score", selection: "3:2", status: BetStatus.WINNER));

            var outcome = transition.CalculateBetOutcome();

            Assert.Equal(1713.6m, outcome.Amount);
            Assert.Equal(outcome.Status, BetOutcomeStatus.Won);
        }

        [Fact]
        public void CalculateBetOutcome_BobLoses_CorrectAmountAndStatus()
        {
            var transition = new BetTransition(
                new Bet(id: 2, amount: 358, odds: 2.73, client: "Bob", @event: "Eve vs Bob", market: "1X2", selection: "2", status: BetStatus.OPEN),
                new Bet(id: 2, amount: 358, odds: 2.73, client: "Bob", @event: "Eve vs Bob", market: "1X2", selection: "2", status: BetStatus.LOSER));

            var outcome = transition.CalculateBetOutcome();

            Assert.Equal(358, outcome.Amount);
            Assert.Equal(outcome.Status, BetOutcomeStatus.Lost);
        }

    }
}
