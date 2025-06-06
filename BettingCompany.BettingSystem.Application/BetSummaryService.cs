using BettingCompany.BettingSystem.Domain;
using BettingCompany.BettingSystem.Domain.Extension;
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
            var betsInMemorySummary = betsInMemory.GetSummary();

            string[] topFiveWinners = CalculateTopFiveWinners(betSummaryFromStorage.TopFiveWinners, betsInMemorySummary.TopFiveWinners);
            string[] topFiveLosers = CalculateTopFiveLosers(betSummaryFromStorage.TopFiveLosers, betsInMemorySummary.TopFiveLosers);

            var totalProfitOrLoss = betSummaryFromStorage.TotalProfitOrLoss + betsInMemorySummary.TotalProfitOrLoss;
            decimal totalAmount = betSummaryFromStorage.TotalAmount + betsInMemorySummary.TotalAmount;
            var totalProcessed = betSummaryFromStorage.TotalProcessed + betsInMemory.Length;

            return new BetSummary(totalProcessed, totalAmount, totalProfitOrLoss, topFiveWinners, topFiveLosers);
        }

        private string[] CalculateTopFiveLosers(string[] topFiveLosers1, string[] topFiveLosers2)
        {
            throw new NotImplementedException();
        }

        private string[] CalculateTopFiveWinners(string[] topFiveWinners1, string[] topFiveWinners2)
        {
            throw new NotImplementedException();
        }
    }
}
