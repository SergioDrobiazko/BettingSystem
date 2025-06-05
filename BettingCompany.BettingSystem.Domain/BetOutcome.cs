namespace BettingCompany.BettingSystem.Domain
{
    public class BetOutcome
    {
        public static BetOutcome Lost(decimal amount)
        {
            var betOutcome = new BetOutcome(BetOutcomeStatus.Lost, amount);
            return betOutcome;
        }

        public static BetOutcome Won(decimal amount)
        {
            var betOutcome = new BetOutcome(BetOutcomeStatus.Won, amount);
            return betOutcome;
        }

        public static BetOutcome Void(decimal amount)
        {
            var betOutcome = new BetOutcome(BetOutcomeStatus.Void, amount);
            return betOutcome;
        }

        private BetOutcome(string status, decimal amount)
        {
            Status = status;
            Amount = amount;
        }

        public decimal Amount { get; private set; }

        public string Status { get; private set; }
    }
}