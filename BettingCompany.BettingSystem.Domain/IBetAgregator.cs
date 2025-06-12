using System;

namespace BettingCompany.BettingSystem.Domain
{
    public interface IBetAgregator
    {
        void AddBet(Bet bet);

        event EventHandler<BetTransitionFormedEventArgs> BetTransitionFormed;

        void ShutDown();
    }
}