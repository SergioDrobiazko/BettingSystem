using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
