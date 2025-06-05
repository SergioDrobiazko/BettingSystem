using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public interface IWorker
    {
        Task<BetCalculated> CalculateBetAsync(BetTransition betTransition, System.Threading.CancellationToken cancellationToken);
    }
}
