using System;
using System.Collections.Generic;

namespace BettingCompany.BettingSystem.Domain
{
    public interface IBetAgregator
    {
        void AddBet(Bet bet);

        bool TryGetBetTransition(out BetTransition betTransition);

        event EventHandler<BetTransitionFormedEventArgs> BetTransitionFormed;
    }
}