namespace BettingCompany.BettingSystem.Domain
{
    public class BetSummary
    {
        public int TotalProcessed { get; init; }
        public int TotalAmount { get; init; }

        public int TotalProfitOrLoss { get; init; }

        public int TopFiveWinners { get; init; }
        public int TopFiveLosers { get; init; }
    }
}