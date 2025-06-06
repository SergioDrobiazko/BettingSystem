using BettingCompany.BettingSystem.Domain;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application
{
    public interface IBetHandlingService
    {
        void Handle(Bet bet);

        void ShutDown();

        Task WhenAllHandled();
    }
}