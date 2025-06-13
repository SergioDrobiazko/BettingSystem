using System;

namespace BettingCompany.BettingSystem.Domain
{
    public interface IBetAggregator
    {
        void AddBet(Bet bet);

        event EventHandler<BetTransitionFormedEventArgs> BetTransitionFormed;

        void ShutDown();
    }
}