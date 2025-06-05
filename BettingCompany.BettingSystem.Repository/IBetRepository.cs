using BettingCompany.BettingSystem.Domain;
using System.Collections.Generic;

namespace BettingCompany.BettingSystem.Repository
{
    public interface IBetRepository
    {
        IEnumerable<BetCalculated> Get();

        void Save(IList<BetCalculated> bets);
    }
}