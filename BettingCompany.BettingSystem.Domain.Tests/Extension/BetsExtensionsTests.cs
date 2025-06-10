using BettingCompany.BettingSystem.Domain.Extension;
using System.Collections.Generic;
using Xunit;

namespace BettingCompany.BettingSystem.Domain.Tests
{
    public class BetsExtensionsTests
    {
        [Fact]
        public void GetSummary()
        {
            var transitions = new List<BetTransition>
            {
                new BetTransition
                (
                    new Bet(id: 1, amount: 358, odds: 2.73, client: "Bob", @event: "Eve vs Bob", market: "1X2", selection: "2", status: BetStatus.OPEN),
                    new Bet(id: 1, amount: 358, odds: 2.73, client: "Bob", @event: "Eve vs Bob", market: "1X2", selection: "2", status: BetStatus.LOSER)
                ),
                new BetTransition
                (
                    new Bet(id: 2, amount: 425, odds: 2.23, client: "Bob", @event: "Sam vs Bob", market: "1X2", selection: "2", status: BetStatus.OPEN),
                    new Bet(id: 2, amount: 425, odds: 2.73, client: "Bob", @event: "Sam vs Bob", market: "1X2", selection: "2", status: BetStatus.LOSER)
                ),
                new BetTransition
                (
                    new Bet(id: 3, amount: 400, odds: 1.13, client: "Bob", @event: "Kat vs Bob", market: "1X2", selection: "2", status: BetStatus.OPEN),
                    new Bet(id: 3, amount: 400, odds: 1.13, client: "Bob", @event: "Kat vs Bob", market: "1X2", selection: "2", status: BetStatus.WINNER)
                ),
                new BetTransition
                (
                    new Bet(id: 4, amount: 510, odds: 1.85, client: "Alice", @event: "Tom vs Jerry", market: "O/U", selection: "Over 2.5", status: BetStatus.OPEN),
                    new Bet(id: 4, amount: 510, odds: 1.85, client: "Alice", @event: "Tom vs Jerry", market: "O/U", selection: "Over 2.5", status: BetStatus.LOSER)
                ),
                new BetTransition
                (
                    new Bet(id: 5, amount: 300, odds: 3.1, client: "Alice", @event: "Neo vs Smith", market: "1X2", selection: "1", status: BetStatus.OPEN),
                    new Bet(id: 5, amount: 300, odds: 3.1, client: "Alice", @event: "Neo vs Smith", market: "1X2", selection: "1", status: BetStatus.WINNER)
                ),
                new BetTransition
                (
                    new Bet(id: 6, amount: 120, odds: 1.45, client: "John", @event: "Ana vs Elsa", market: "1X2", selection: "X", status: BetStatus.OPEN),
                    new Bet(id: 6, amount: 120, odds: 1.45, client: "John", @event: "Ana vs Elsa", market: "1X2", selection: "X", status: BetStatus.WINNER)
                ),
                new BetTransition
                (
                    new Bet(id: 7, amount: 560, odds: 2.0, client: "Eve", @event: "Max vs Leo", market: "BTTS", selection: "Yes", status: BetStatus.OPEN),
                    new Bet(id: 7, amount: 560, odds: 2.0, client: "Eve", @event: "Max vs Leo", market: "BTTS", selection: "Yes", status: BetStatus.LOSER)
                ),
                new BetTransition
                (
                    new Bet(id: 8, amount: 220, odds: 1.9, client: "Eve", @event: "Team A vs Team B", market: "1X2", selection: "1", status: BetStatus.OPEN),
                    new Bet(id: 8, amount: 220, odds: 1.9, client: "Eve", @event: "Team A vs Team B", market: "1X2", selection: "1", status: BetStatus.WINNER)
                ),
                new BetTransition
                (
                    new Bet(id: 9, amount: 1000, odds: 5.5, client: "John", @event: "Big Match", market: "Correct Score", selection: "2-1", status: BetStatus.OPEN),
                    new Bet(id: 9, amount: 1000, odds: 5.5, client: "John", @event: "Big Match", market: "Correct Score", selection: "2-1", status: BetStatus.LOSER)
                ),
                new BetTransition
                (
                    new Bet(id: 10, amount: 50, odds: 10.0, client: "Sam", @event: "Surprise Match", market: "1X2", selection: "X", status: BetStatus.OPEN),
                    new Bet(id: 10, amount: 50, odds: 10.0, client: "Sam", @event: "Surprise Match", market: "1X2", selection: "X", status: BetStatus.WINNER)
                )
            };

            var summary = transitions.GetSummary();

            Assert.Equal(10, summary.TotalProcessed);
            Assert.Equal(-1469, summary.TotalProfitOrLoss);
            Assert.Equal(3943, summary.TotalAmount);
        }
    }
}
