namespace BettingCompany.BettingSystem.Domain
{
    public class BetSummary
    {
        public BetSummary(int totalProcessed, decimal totalAmount, decimal totalProfitOrLoss, ClientProfit[] topFiveWinners, ClientProfit[] topFiveLosers)
        {
            TotalProcessed = totalProcessed;
            TotalAmount = totalAmount;
            TotalProfitOrLoss = totalProfitOrLoss;
            TopFiveWinners = topFiveWinners;
            TopFiveLosers = topFiveLosers;
        }

        public int TotalProcessed { get; }
        public decimal TotalAmount { get; }

        public decimal TotalProfitOrLoss { get; }

        public ClientProfit[] TopFiveWinners { get; }
        public ClientProfit[] TopFiveLosers { get; }
    }
}