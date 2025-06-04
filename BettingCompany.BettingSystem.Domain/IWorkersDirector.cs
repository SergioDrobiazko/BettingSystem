using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public interface IWorkersDirector
    {
        void DelegateWork(BetTransition betTransition);
        BetCalculated? FetchCalculatedBet();

        event EventHandler<BetCalculatedEventArgs> BetCalculated;
    }
}
