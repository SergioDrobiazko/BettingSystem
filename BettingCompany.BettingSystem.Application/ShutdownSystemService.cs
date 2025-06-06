using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
