using BettingCompany.BettingSystem.Domain;
using System.Collections.Generic;

namespace BettingCompany.BettingSystem.Repository
{
    public interface IBetRepository
    {
        IEnumerable<Bet> Get()
        {

        }

        void Save(IEnumerable<Bet> bets);
    }
}