namespace BettingCompany.BettingSystem.Domain
{
    public class BetSummary
    {
        public int TotalProcessed { get; init; }
        public double TotalAmount { get; init; }

        public decimal TotalProfitOrLoss { get; init; }

        public string[] TopFiveWinners { get; init; }
        public string[] TopFiveLosers { get; init; }
    }
}