namespace BettingCompany.BettingSystem.Domain
{
    public class BetDataSummary
    {
        public int TotalProcessed { get; private set; }
        public int TotalAmount { get; private set; }

        public int TotalProfitOrLoss { get; private set; }

        public int TopFiveWinners { get; private set; }
        public int TopFiveLosers { get; private set; }
    }
}