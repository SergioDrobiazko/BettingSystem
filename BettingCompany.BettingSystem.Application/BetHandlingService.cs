using BettingCompany.BettingSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application
{
    public class BetHandlingService
    {
        private readonly IBetAgregator betAgregator;
        private readonly IWorkersDirector workersDirector;

        public void Handle(Bet bet)
        {
            betAgregator.AddBet(bet);
            workersDirector.DelegateWork(betAgregator);
        }
    }
}
