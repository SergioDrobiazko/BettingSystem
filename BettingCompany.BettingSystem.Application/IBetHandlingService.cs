using BettingCompany.BettingSystem.Domain;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application
{
    public interface IBetHandlingService
    {
        Task HandleAsync(Bet bet);

        void ShutDown();

        Task WhenAllHandled();
    }
}