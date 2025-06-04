using BettingCompany.BettingSystem.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application
{
    public class PersistancePolicy : IPersistancePolicy
    {
        public bool ShouldPersist(ConcurrentQueue<BetCalculated> betsCalculated)
        {
            return betsCalculated.Count > 0;
        }

        public int GetNumberOfElementsToSave()
        {
            return 1;
        }
    }
}
