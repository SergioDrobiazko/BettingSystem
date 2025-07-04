﻿using System.Collections.Generic;
using System.Linq;

namespace BettingCompany.BettingSystem.Domain.Extension
{
    public static class BetsExtensions
    {
        public static BetSummary GetSummary(this IEnumerable<BetCalculated> bets)
        {
            var validBets = bets.Where(bet => bet.IsMarkedForReview == false);

            var groupsByUser = validBets
                .GroupBy(x => x.BetTransition.InitialBet.Client);

            var usersProfits = groupsByUser
                .Select(x => new ClientProfit(x.Key, x.Sum(user => user.GetProfit())))
                .OrderBy(x => x.Profit);

            var topFiveWinners = usersProfits.TakeLast(5).ToArray();
            var topFiveLosers = usersProfits.Take(5).ToArray();

            var totalProfitOrLoss = usersProfits.Sum(x => x.Profit);

            var totalBetsProcessed = validBets.Count();
            var totalAmount = validBets.Sum(x => (decimal)x.BetTransition.InitialBet.Amount);

            return new BetSummary(totalBetsProcessed, totalAmount, totalProfitOrLoss, topFiveWinners, topFiveLosers);
        }

        public static BetSummary GetSummary(this IEnumerable<BetTransition> bets)
        {
            var betsCalculated = bets
                .Where(bet => bet.IsValidStatusTransition())
                .Select(bet => new BetCalculated(bet, bet.CalculateBetOutcome()));

            var summary = betsCalculated
                .GetSummary();
            return summary;
        }
    }
}
