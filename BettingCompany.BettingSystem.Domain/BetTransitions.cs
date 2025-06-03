using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class BetTransitions
    {
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
    }
}
