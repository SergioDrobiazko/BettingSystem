using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class Worker : IWorker
    {
        public Task<BetCalculated> CalculateBet(BetTransition betTransition)
        {
            throw new NotImplementedException();
        }
    }
}
