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

        public BetTransition BetTransition { get; set; }

        public BetOutcome BetOutcome { get; private set; }

        public bool IsMarkedForReview { get; private set; }
        public ObjectId Id { get; set; }

        public decimal GetProfit()
        {
            var status = BetOutcome.Status;

            if(status == BetOutcomeStatus.Won)
            {
                return BetOutcome.Amount - (decimal)BetTransition.InitialBet.Amount;
            }

            if(status == BetOutcomeStatus.Lost)
            {
                return -BetOutcome.Amount;
            }

            if(status == BetOutcomeStatus.Void)
            {
                return 0;
            }

            throw new ArgumentException();
        }
    }
}
