using BettingCompany.BettingSystem.Application;
using BettingCompany.BettingSystem.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddBetController : ControllerBase
    {
        private readonly ILogger<AddBetController> _logger;

        private readonly IBetHandlingService _betHandlingService;

        public AddBetController(ILogger<AddBetController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void AddBet(Bet bet)
        {
            _betHandlingService.Handle(bet);
        }
    }
}
