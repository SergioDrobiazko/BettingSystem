using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public interface IWorkersDirector
    {
        void DelegateBetCalculation(BetTransition betTransition);
        BetCalculated? FetchCalculatedBet();
        BetCalculated[] GetBetsCalculatedSnapshot();
        Task WhenAllBetsCalculated();

        event EventHandler<BetCalculatedEventArgs> BetCalculated;

        void ShutDown();
    }
}
