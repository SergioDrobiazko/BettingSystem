using BettingCompany.BettingSystem.Domain;
using BettingCompany.BettingSystem.Domain.Extension;
using BettingCompany.BettingSystem.Repository;
using System;
using System.Linq;

namespace BettingCompany.BettingSystem.Application
{
    public class BetSummaryService : IBetSummaryService
    {
        private readonly IBetRepository _betRepository;
        private readonly IBetHandlingService _betHandlingService;

        public BetSummaryService(IBetRepository betRepository, IBetHandlingService betHandlingService)
        {
            _betRepository = betRepository;
            _betHandlingService = betHandlingService;
        }

        public BetSummary GetSummary()
        {
            BetSummary betSummaryFromStorage = default;
            BetCalculated[] betsInMemory;

            lock (StorageLock.Lock)
            {
                betSummaryFromStorage = _betRepository.GetSummary();

                betsInMemory = _betHandlingService.GetBetsSnapshot();
            }

            BetSummary betSummary = CalculateBetSummary(betSummaryFromStorage, betsInMemory);

            return betSummary;
        }

        private BetSummary CalculateBetSummary(BetSummary betSummaryFromStorage, BetCalculated[] betsInMemory)
        {
            var betsInMemorySummary = betsInMemory.GetSummary();

            ClientProfit[] topFiveWinners = CalculateTopFiveWinners(betSummaryFromStorage.TopFiveWinners, betsInMemorySummary.TopFiveWinners);
            ClientProfit[] topFiveLosers = CalculateTopFiveLosers(betSummaryFromStorage.TopFiveLosers, betsInMemorySummary.TopFiveLosers);

            var totalProfitOrLoss = betSummaryFromStorage.TotalProfitOrLoss + betsInMemorySummary.TotalProfitOrLoss;
            decimal totalAmount = betSummaryFromStorage.TotalAmount + betsInMemorySummary.TotalAmount;
            var totalProcessed = betSummaryFromStorage.TotalProcessed + betsInMemory.Length;

            return new BetSummary(totalProcessed, totalAmount, totalProfitOrLoss, topFiveWinners, topFiveLosers);
        }

        private ClientProfit[] CalculateTopFiveLosers(ClientProfit[] topFiveLosers1, ClientProfit[] topFiveLosers2)
        {
            return topFiveLosers1
                .Union(topFiveLosers2)
                .OrderBy(x => x.Profit)
                .Take(5)
                .ToArray();
        }

        private ClientProfit[] CalculateTopFiveWinners(ClientProfit[] topFiveWinners1, ClientProfit[] topFiveWinners2)
        {
            return topFiveWinners1
                .Union(topFiveWinners2)
                .OrderBy(x => x.Profit)
                .TakeLast(5)
                .ToArray();
        }
    }
}
