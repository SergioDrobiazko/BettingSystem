using BettingCompany.BettingSystem.Domain;
using System.Collections.Generic;

namespace BettingCompany.BettingSystem.Repository
{
    public interface IBetRepository
    {
        void Save(IEnumerable<Bet> bets);
    }
}