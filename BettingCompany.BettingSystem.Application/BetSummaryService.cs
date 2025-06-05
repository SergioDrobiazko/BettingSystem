using BettingCompany.BettingSystem.Domain;
using BettingCompany.BettingSystem.Repository;
using System;
using System.Linq;

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

            var totalProfitOrLoss = CalculateTopProfitOrLoss(betSummaryFromStorage, betsInMemory);
            double totalAmount = CalculateTotalAmount(betSummaryFromStorage, betsInMemory);
            var totalProcessed = CalculateTotalProcessed(betSummaryFromStorage, betsInMemory);

            return new BetSummary()
            {
                TopFiveWinners = topFiveWinners,
                TopFiveLosers = topFiveLosers,
                TotalProfitOrLoss = totalProfitOrLoss,
                TotalAmount = totalAmount,
                TotalProcessed = totalProcessed
            };
        }

        private double CalculateTotalAmount(BetSummary betSummaryFromStorage, BetCalculated[] betsInMemory)
        {
            throw new NotImplementedException();
        }

        private int CalculateTotalProcessed(BetSummary betSummaryFromStorage, BetCalculated[] betsInMemory)
        {
            return betSummaryFromStorage.TotalProcessed + betsInMemory.Length;
        }

        private decimal CalculateTopProfitOrLoss(BetSummary betSummaryFromStorage, BetCalculated[] betsInMemory)
        {
            var partialSumOfWins = betsInMemory
                .Where(x => x.BetOutcome.Status == BetOutcomeStatus.Won)
                .Sum(x => x.BetOutcome.Amount);

            var partialSumOfLoses = betsInMemory
                .Where(x => x.BetOutcome.Status == BetOutcomeStatus.Lost)
                .Sum(x => (-1) * x.BetOutcome.Amount);

            var topProfitOrLoss = betSummaryFromStorage.TotalProfitOrLoss + partialSumOfWins + partialSumOfLoses;

            return topProfitOrLoss;
        }

        private string[] CalculateTopFiveLosers(BetSummary betSummaryFromStorage, BetCalculated[] betsInMemory)
        {
            throw new NotImplementedException();
        }

        private string[] CalculateTopFiveWinners(BetSummary betSummaryFromStorage, BetCalculated[] betsInMemory)
        {
            throw new NotImplementedException();
        }
    }
}
