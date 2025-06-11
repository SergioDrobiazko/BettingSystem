using BettingCompany.BettingSystem.Application;
using BettingCompany.BettingSystem.Application.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShutDownSystemController : ControllerBase
    {
        private readonly IShutdownSystemService _shutdownSystemService;

        private readonly IHostApplicationLifetime _lifetime;

        public ShutDownSystemController(IShutdownSystemService shutdownSystemService, IHostApplicationLifetime lifetime)
        {
            _shutdownSystemService = shutdownSystemService;
            _lifetime = lifetime;
        }

        [HttpPost()]
        public void Shutdown()
        {
            _shutdownSystemService.Shutdown();
            _lifetime.StopApplication();
        }
    }
}
