using System.Collections.Generic;
using System.Linq;

namespace BettingCompany.BettingSystem.Domain.Extension
{
    public static class BetsExtensions
    {
        public static BetSummary GetSummary(this IEnumerable<BetCalculated> bets)
        {
            var groupsByUser = bets
                .GroupBy(x => x.BetTransition.InitialBet.Client);

            var usersProfits = groupsByUser
                .Select(x => new ClientProfit(x.Key, x.Sum(user => user.BetOutcome.Amount)))
                .OrderBy(x => x.Profit);

            var topFiveWinners = usersProfits.TakeLast(5).ToArray();
            var topFiveLosers = usersProfits.Take(5).ToArray();

            var totalProfitOrLoss = usersProfits.Sum(x => x.Profit);

            var totalBetsProcessed = bets.Count();
            var totalAmount = bets.Sum(x => x.GetProfit());

            return new BetSummary(totalBetsProcessed, totalAmount, totalProfitOrLoss, topFiveWinners, topFiveLosers);
        }
    }
}
