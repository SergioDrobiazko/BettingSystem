using BettingCompany.BettingSystem.Domain;
using BettingCompany.BettingSystem.Repository;
using System;

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
            BetSummary betSummaryFromStorage = default;
            BetCalculated[] betsInMemory;

            lock (StorageLock.Lock)
            {
                betSummaryFromStorage = _betRepository.GetSummary();

                betsInMemory = _workersDirector.GetBetsCalculatedSnapshot();
            }

            BetSummary betSummary = CalculateBetSummary(betSummaryFromStorage, betsInMemory);

            return betSummary;
        }

        private BetSummary CalculateBetSummary(BetSummary betSummaryFromStorage, BetCalculated[] betsInMemory)
        {
            var topFiveWinners = CalculateTopFiveWinners(betSummaryFromStorage, betsInMemory);
            var topFiveLosers = CalculateTopFiveLosers(betSummaryFromStorage, betsInMemory);

        }

        private object CalculateTopFiveLosers(BetSummary betSummaryFromStorage, BetCalculated[] betsInMemory)
        {
            throw new NotImplementedException();
        }

        private object CalculateTopFiveWinners(BetSummary betSummaryFromStorage, BetCalculated[] betsInMemory)
        {
            throw new NotImplementedException();
        }
    }
}
