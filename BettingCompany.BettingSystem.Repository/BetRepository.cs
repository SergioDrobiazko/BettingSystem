using BettingCompany.BettingSystem.Domain;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace BettingCompany.BettingSystem.Repository
{
    public class BetRepository : IBetRepository
    {
        private readonly IMongoCollection<BetCalculated> _bets;

        public BetRepository(IMongoDatabase mongoDatabase)
        {
            _bets = mongoDatabase.GetCollection<BetCalculated>("Bets");
        }

        public IEnumerable<BetCalculated> Get()
        {
            return _bets.Find(_ => true).ToList();
        }

        public void Save(IList<BetCalculated> bets)
        {
            _bets.InsertMany(bets);
        }
    }
}
