using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class BetTransition
    {
        public BetTransition(Bet initialBet, Bet secondaryBet)
        {
            InitialBet = initialBet;
            SecondaryBet = secondaryBet;
        }

        public Bet InitialBet { get; private set; }

        public Bet SecondaryBet { get; private set; }

        public bool IsValidStatusTransition()
        {
            bool isValid = InitialBet.Status == BetStatus.OPEN &&
                (
                    SecondaryBet.Status == BetStatus.WINNER
                        || SecondaryBet.Status == BetStatus.LOSER
                        || SecondaryBet.Status == BetStatus.VOID
                );

            return isValid;
        }

        public BetOutcome CalculateBetOutcome()
        {
            BetOutcome betOutcome = default;

            if(SecondaryBet.Status == BetStatus.WINNER)
            {
                betOutcome = CalculateWinOutcome();
            }
            else if(SecondaryBet.Status == BetStatus.LOSER)
            {
                betOutcome = CalculateLoseOutcome();
            }
            else if(SecondaryBet.Status == BetStatus.VOID)
            {
                betOutcome = CalculateVoidOutcome();
            }

            return betOutcome;
        }

        private BetOutcome CalculateVoidOutcome()
        {
            return BetOutcome.Void((decimal)SecondaryBet.Amount);
        }

        private BetOutcome CalculateLoseOutcome()
        {
            return BetOutcome.Lost((decimal)SecondaryBet.Amount);
        }

        private BetOutcome CalculateWinOutcome()
        {
            return BetOutcome.Won((decimal)SecondaryBet.Amount * (decimal)SecondaryBet.Odds);
        }
    }
}
