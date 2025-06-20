﻿using BettingCompany.BettingSystem.Application.Contract;
using BettingCompany.BettingSystem.Domain;
using MethodTimer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BettingCompany.BettingSystem.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BetSummaryController : ControllerBase
    {
        private readonly ILogger<BetSummaryController> _logger;

        private readonly IBetSummaryService _betSummaryService;

        public BetSummaryController(IBetSummaryService betSummaryService, ILogger<BetSummaryController> logger)
        {
            _betSummaryService = betSummaryService;
            _logger = logger;
        }

        [HttpGet]
        [Time]
        public BetSummary GetSummary()
        {
            var summary = _betSummaryService.GetSummary();

            return summary;
        }
    }
}
