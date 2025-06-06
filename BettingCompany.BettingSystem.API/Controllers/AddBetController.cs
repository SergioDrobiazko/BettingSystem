using BettingCompany.BettingSystem.Application;
using BettingCompany.BettingSystem.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BettingCompany.BettingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddBetController : ControllerBase
    {
        private readonly ILogger<AddBetController> _logger;

        private readonly IBetHandlingService _betHandlingService;

        public AddBetController(ILogger<AddBetController> logger, IBetHandlingService betHandlingService)
        {
            _logger = logger;
            _betHandlingService = betHandlingService;
        }

        [HttpPost]
        public void AddBet([FromBody]Bet bet)
        {
            _betHandlingService.Handle(bet);
        }
    }
}
