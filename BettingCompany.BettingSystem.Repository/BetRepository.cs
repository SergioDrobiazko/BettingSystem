using BettingCompany.BettingSystem.Domain;
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

            var groupsByUser = bets
                .GroupBy(x => x.BetTransition.InitialBet.Client);

            var usersProfits = groupsByUser
                .Select(x => new { user = x.Key, totalProfit = x.Sum(user => user.BetOutcome.Amount) })
                .OrderBy(x => x.totalProfit);

            var topFiveWinners = usersProfits.Take(5).Select(x => x.user).ToArray();
            var topFiveLosers = usersProfits.TakeLast(5).Select(x => x.user).ToArray();

            var totalProfitOrLoss = usersProfits.Sum(x => x.totalProfit);

            var totalBetsProcessed = bets.Count();
            var totalAmount = bets.Sum(x => x.GetProfit());

            return new BetSummary(totalBetsProcessed, totalAmount, totalProfitOrLoss, topFiveWinners, topFiveLosers);
        }

        public void Save(IList<BetCalculated> bets)
        {
            _bets.InsertMany(bets);
        }
    }
}
