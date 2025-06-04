using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class Worker : IWorker
    {
        public async Task<BetCalculated> CalculateBetAsync(BetTransition betTransition)
        {
            return await Task.Run(() => CalculateBet(betTransition));
        }

        private BetCalculated CalculateBet(BetTransition betTransition)
        {
            if (betTransition.IsValidStatusTransition() == false)
            {
                return BetCalculated.MarkedForReview(betTransition);
            }

            var betOutcome = betTransition.CalculateBetOutcome();

            return new BetCalculated(betTransition, betOutcome);
        }
    }
}
