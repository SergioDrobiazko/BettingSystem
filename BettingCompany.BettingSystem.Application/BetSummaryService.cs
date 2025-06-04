using BettingCompany.BettingSystem.Domain;
using BettingCompany.BettingSystem.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application
{
    public class BetSummaryService : IBetSummaryService
    {
        private readonly IBetRepository _betRepository;
        private readonly IWorkersDirector _workersDirector;

        public BetSummaryService(IBetRepository betRepository, IWorkersDirector workersDirector)
        {
            _betRepository = betRepository;
            _workersDirector = workersDirector;
        }

        public BetSummary GetSummary()
        {
            lock(StorageLock.Lock)
            {
                var betsFromStorage = _betRepository.Get();

                var betsInMemory = _workersDirector.CopyBetsCalculated();

                BetSummary betSummary = CalculateBetSummary(betsFromStorage, betsInMemory);

                return betSummary;
            }
        }

        private BetSummary CalculateBetSummary(IEnumerable<Bet> betsFromStorage, BetCalculated[] betsInMemory)
        {
            throw new NotImplementedException();
        }
    }
}
