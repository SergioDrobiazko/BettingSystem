using BettingCompany.BettingSystem.Domain;
using System;
using System.Collections.Generic;

namespace BettingCompany.BettingSystem.Repository
{
    public class BetRepository : IBetRepository
    {
        public IEnumerable<Bet> Get()
        {
            throw new NotImplementedException();
        }

        public void Save(IList<BetCalculated> bets)
        {
            throw new NotImplementedException();
        }
    }
}
