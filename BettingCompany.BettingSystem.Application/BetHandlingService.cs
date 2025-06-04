using BettingCompany.BettingSystem.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BettingCompany.BettingSystem.Application
{
    public class BetHandlingService : IBetHandlingService
    {
        private readonly IBetAgregator _betAgregator;
        private readonly IWorkersDirector _workersDirector;

        private ConcurrentQueue<BetCalculated> betsCalculated = new();

        public BetHandlingService(IBetAgregator betAgregator, IWorkersDirector workersDirector)
        {
            _betAgregator = betAgregator;
            _betAgregator.BetTransitionFormed += OnBetTransitionFormed;

            _workersDirector = workersDirector;
            _workersDirector.BetCalculated += OnBetCalculated;
        }

        private void OnBetTransitionFormed(object sender, BetTransitionFormedEventArgs e)
        {
            _workersDirector.DelegateWork(e.BetTransition);
        }

        private void OnBetCalculated(object sender, BetCalculatedEventArgs e)
        {
            var calculatedBet = _workersDirector.FetchCalculatedBet();
            if(calculatedBet != null)
            {
                betsCalculated.Enqueue(calculatedBet);
            }
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
