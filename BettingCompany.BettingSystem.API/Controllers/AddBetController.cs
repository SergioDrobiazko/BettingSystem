using System.Threading.Tasks;
using BettingCompany.BettingSystem.Application.Contract;
using BettingCompany.BettingSystem.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BettingCompany.BettingSystem.API.Controllers
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
        public async Task AddBet([FromBody]Bet bet)
        {
            await _betHandlingService.HandleAsync(bet);
        }
    }
}
