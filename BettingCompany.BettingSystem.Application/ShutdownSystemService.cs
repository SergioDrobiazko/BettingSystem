using BettingCompany.BettingSystem.Application.Contract;

namespace BettingCompany.BettingSystem.Application
{
    public class ShutdownSystemService : IShutdownSystemService
    {
        private readonly IBetHandlingService _betHandlingService;
        public ShutdownSystemService(IBetHandlingService betHandlingService)
        {
            _betHandlingService = betHandlingService;
        }

        public void Shutdown()
        {
            _betHandlingService.ShutDown();
        }
    }
}
