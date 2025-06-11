using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class Worker : IWorker
    {
        public async Task<BetCalculated> CalculateBetAsync(BetTransition betTransition, CancellationToken cancellationToken)
        {
            await Task.Delay(new Random().Next(1000, 60000));
            return await Task.Run(() => CalculateBet(betTransition, cancellationToken), cancellationToken);
        }

        private BetCalculated CalculateBet(BetTransition betTransition, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (betTransition.IsValidStatusTransition() == false)
            {
                return BetCalculated.MarkedForReview(betTransition);
            }

            var betOutcome = betTransition.CalculateBetOutcome();

            return new BetCalculated(betTransition, betOutcome);
        }
    }
}
