using BettingCompany.BettingSystem.Application;
using BettingCompany.BettingSystem.Domain;
using Microsoft.AspNetCore.Http;
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
    public class BetSummaryController : ControllerBase
    {
        private readonly ILogger<BetSummaryController> _logger;

        private readonly IBetSummaryService _betSummaryService;

        [HttpGet]
        public BetSummary GetSummary()
        {
            var summary = _betSummaryService.GetSummary();

            return summary;
        }
    }
}
