using BettingCompany.BettingSystem.Domain;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application.Contract
{
    public interface IBetHandlingService
    {
        BetCalculated[] GetBetsSnapshot();
        Task HandleAsync(Bet bet);

        void ShutDown();

        Task WhenAllHandled();
    }
}
