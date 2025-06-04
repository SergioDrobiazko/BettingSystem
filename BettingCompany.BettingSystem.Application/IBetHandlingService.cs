using BettingCompany.BettingSystem.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application
{
    public interface IBetHandlingService
    {
        void Handle(Bet bet);
        Task HandleAsync(Bet bet);
        void SaveBets(IEnumerable<BetCalculated> bets);
    }
}