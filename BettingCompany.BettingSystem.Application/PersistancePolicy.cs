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
        public int GetNumberOfElementsToSave()
        {
            return 100;
        }

        public bool ShouldPersist(int betsCalculated)
        {
            return betsCalculated == 100;
        }
    }
}
