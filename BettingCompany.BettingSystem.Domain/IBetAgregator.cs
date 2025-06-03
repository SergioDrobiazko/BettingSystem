using System.Collections.Generic;

namespace BettingCompany.BettingSystem.Domain
{
    public interface IBetAgregator
    {
        void AddBet(Bet bet);

        IEnumerable<BetTransitions> GetBetTransitions();
    }
}