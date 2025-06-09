using BettingCompany.BettingSystem.Application;
using BettingCompany.BettingSystem.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddBetsController : ControllerBase
    {
        private readonly ILogger<AddBetsController> _logger;

        private readonly IBetHandlingService _betHandlingService;

        public AddBetsController(IBetHandlingService betHandlingService)
        {
            _betHandlingService = betHandlingService;
        }

        [HttpPost]
        public async Task AddBets(IEnumerable<Bet> bets)
        {
            foreach(var bet in bets)
            {
                await _betHandlingService.HandleAsync(bet);
            }
        }
    }
}
