using BettingCompany.BettingSystem.Application.Contract;
using BettingCompany.BettingSystem.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MethodTimer;

namespace BettingCompany.BettingSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddBetsController : ControllerBase
    {
        private readonly ILogger<AddBetsController> _logger;

        private readonly IBetHandlingService _betHandlingService;

        public AddBetsController(IBetHandlingService betHandlingService, ILogger<AddBetsController> logger)
        {
            _betHandlingService = betHandlingService;
            _logger = logger;
        }

        [HttpPost]
        [Time]
        public async Task AddBets(IEnumerable<Bet> bets)
        {
            foreach(var bet in bets)
            {
                await _betHandlingService.HandleAsync(bet);
            }

            _logger.LogDebug($"Added bunch of bets"); 
        }
    }
}
