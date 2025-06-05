using BettingCompany.BettingSystem.Domain;
using BettingCompany.BettingSystem.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application
{
    public class BetsChunkCalculated : EventArgs
    {
        public ConcurrentQueue<BetCalculated> betsCalculated;

        public BetsChunkCalculated(ConcurrentQueue<BetCalculated> betsCalculated)
        {
            this.betsCalculated = betsCalculated;
        }
    }

    public class BetHandlingService : IBetHandlingService
    {
        private readonly IBetAgregator _betAgregator;
        private readonly IWorkersDirector _workersDirector;
        private readonly IPersistancePolicy _persistancePolicy;
        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly IBetRepository _betRepository;

        private ConcurrentQueue<BetCalculated> betsCalculated = new();

        private int betsCalculatedCounter = 0;

        private readonly object betsCalculatedCounterLock = new object();

        public event EventHandler<BetsChunkCalculated> ChunkCalculated;

        public BetHandlingService(
            IBetAgregator betAgregator,
            IWorkersDirector workersDirector,
            IPersistancePolicy persistancePolicy,
            IDateTimeProvider dateTimeProvider,
            IBetRepository betRepository)
        {
            _betAgregator = betAgregator;
            _betAgregator.BetTransitionFormed += OnBetTransitionFormed;

            _workersDirector = workersDirector;
            _workersDirector.BetCalculated += OnBetCalculated;
            _persistancePolicy = persistancePolicy;
            _dateTimeProvider = dateTimeProvider;
            _betRepository = betRepository;
        }

        private void OnBetTransitionFormed(object sender, BetTransitionFormedEventArgs e)
        {
            _workersDirector.DelegateBetCalculation(e.BetTransition);
        }

        private void OnBetCalculated(object sender, BetCalculatedEventArgs e)
        {
            var calculatedBet = _workersDirector.FetchCalculatedBet();
            if (calculatedBet != null)
            {
                betsCalculated.Enqueue(calculatedBet);
                lock (betsCalculatedCounterLock)
                {
                    betsCalculatedCounter++;
                }
            }

            if (_persistancePolicy.ShouldPersist(betsCalculatedCounter))
            {
                lock (StorageLock.Lock)
                {
                    int elementsToSave = _persistancePolicy.GetNumberOfElementsToSave();

                    var betsToSave = new BetCalculated[elementsToSave];

                    for (int i = 0; i < elementsToSave; ++i)
                    {
                        betsCalculated.TryDequeue(out var betCalculated);
                        betsToSave[i] = betCalculated;
                    }
                    _betRepository.Save(betsToSave);
                }
            }
        }

        public void Handle(Bet bet)
        {
            bet.SetDateArrived(_dateTimeProvider.GetUTCNow());
            _betAgregator.AddBet(bet);
        }
    }
}
