using BettingCompany.BettingSystem.Application.Contract;
using Microsoft.Extensions.Logging;

namespace BettingCompany.BettingSystem.Application
{
    public class ShutdownSystemService : IShutdownSystemService
    {
        private readonly IBetHandlingService _betHandlingService;

        private readonly ILogger<ShutdownSystemService> _logger;

        public ShutdownSystemService(IBetHandlingService betHandlingService, ILogger<ShutdownSystemService> logger)
        {
            _betHandlingService = betHandlingService;
            _logger = logger;
        }

        public void Shutdown()
        {
            _logger.LogInformation("System shutdown start..");
            _betHandlingService.ShutDown();
        }
    }
}
