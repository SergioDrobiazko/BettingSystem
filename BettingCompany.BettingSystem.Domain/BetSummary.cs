namespace BettingCompany.BettingSystem.Domain
{
    public class BetSummary
    {
        public BetSummary(int totalProcessed, decimal totalAmount, decimal totalProfitOrLoss, string[] topFiveWinners, string[] topFiveLosers)
        {
            TotalProcessed = totalProcessed;
            TotalAmount = totalAmount;
            TotalProfitOrLoss = totalProfitOrLoss;
            TopFiveWinners = topFiveWinners;
            TopFiveLosers = topFiveLosers;
        }

        public int TotalProcessed { get; init; }
        public decimal TotalAmount { get; init; }

        public decimal TotalProfitOrLoss { get; init; }

        public string[] TopFiveWinners { get; init; }
        public string[] TopFiveLosers { get; init; }
    }
}