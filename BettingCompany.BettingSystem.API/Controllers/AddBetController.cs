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
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<AddBetController> _logger;

        public AddBetController(ILogger<AddBetController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void AddBet(Bet bet)
        {
            throw new NotImplementedException();
        }
    }
}
