using BettingCompany.BettingSystem.Domain;
using BettingCompany.BettingSystem.Domain.Extension;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

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

        public BetSummary GetSummary()
        {
            //todo: rewrite to aggregated query to mongo db

            var bets = Get();

            return bets.GetSummary();
        }

        public void Save(IList<BetCalculated> bets)
        {
            _bets.InsertMany(bets);
        }
    }
}
