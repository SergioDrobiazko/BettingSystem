using BettingCompany.BettingSystem.Domain;
using System;
using System.Collections.Generic;

namespace BettingCompany.BettingSystem.Application
{
    public class BetHandlingService : IBetHandlingService
    {
        private readonly IBetAgregator _betAgregator;
        private readonly IWorkersDirector _workersDirector;

        public BetHandlingService(IBetAgregator betAgregator, IWorkersDirector workersDirector)
        {
            _betAgregator = betAgregator;
            _betAgregator.BetTransitionFormed += OnBetTransitionFormed;

            _workersDirector = workersDirector;
            _workersDirector.BetCalculated += OnBetCalculated;
        }

        private void OnBetTransitionFormed(object sender, BetTransitionFormedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnBetCalculated(object sender, BetCalculatedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Handle(Bet bet)
        {
            _betAgregator.AddBet(bet);
        }

        public void SaveBets(IEnumerable<BetCalculated> bets)
        {
            throw new NotImplementedException();
        }
    }
}
