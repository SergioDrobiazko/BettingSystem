using BettingCompany.BettingSystem.Application.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace BettingCompany.BettingSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
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
