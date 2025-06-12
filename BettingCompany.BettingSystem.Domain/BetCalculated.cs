using MongoDB.Bson;
using System;

namespace BettingCompany.BettingSystem.Domain
{
    public class BetCalculated
    {
        public static BetCalculated MarkedForReview(BetTransition betTransition)
        {
            return new BetCalculated(betTransition, isMarkedForReview: true);
        }

        public BetCalculated(BetTransition betTransition, BetOutcome betOutcome)
        {
            BetTransition = betTransition;
            BetOutcome = betOutcome;
        }

        private BetCalculated(BetTransition betTransition, bool isMarkedForReview)
        {
            BetTransition = betTransition;
            IsMarkedForReview = isMarkedForReview;
        }

        public BetTransition BetTransition { get; }

        public BetOutcome BetOutcome { get; }

        public bool IsMarkedForReview { get; private set; }
        public ObjectId Id { get; set; }

        public decimal GetProfit()
        {
            var status = BetOutcome.Status;

            return status switch
            {
                BetOutcomeStatus.Won => BetOutcome.Amount - (decimal)BetTransition.InitialBet.Amount,
                BetOutcomeStatus.Lost => -BetOutcome.Amount,
                BetOutcomeStatus.Void => 0,
                _ => throw new ArgumentException()
            };
        }
    }
}
