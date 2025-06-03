using BettingCompany.BettingSystem.Domain;
using System.Collections.Generic;

namespace BettingCompany.BettingSystem.Application
{
    public interface IBetHandlingService
    {
        void Handle(Bet bet);
        void SaveBets(IEnumerable<BetCalculated> bets);
    }
}