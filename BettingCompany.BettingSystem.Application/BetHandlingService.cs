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
        private bool isShuttingDown = false;

        private readonly IBetAgregator _betAgregator;
        private readonly IWorkersDirector _workersDirector;
        private readonly IPersistancePolicy _persistancePolicy;
        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly IBetRepository _betRepository;

        private ConcurrentQueue<BetCalculated> betsCalculated = new();

        private int incomingBets = 0;
        private int betsHandled = 0;
        private int betsSaved = 0;

        private readonly object betsCalculatedCounterLock = new object();
        private readonly object incomingBetsLock = new object();

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
                    betsHandled++;
                }
            }

            //if (_persistancePolicy.ShouldPersist(betsHandled, betsSaved))
            {
                lock (StorageLock.Lock)
                {
                    int elementsToSave = 1;

                    var betsToSave = new BetCalculated[elementsToSave];

                    for (int i = 0; i < elementsToSave; ++i)
                    {
                        betsCalculated.TryDequeue(out var betCalculated);
                        betsToSave[i] = betCalculated;
                    }
                    _betRepository.Save(betsToSave);
                    betsSaved += elementsToSave;
                }
            }
        }

        private ConcurrentQueue<Bet> unhandledBets = new();

        public void Handle(Bet bet)
        {
            lock (incomingBetsLock)
            {
                incomingBets++;
            }

            if (isShuttingDown)
            {
                unhandledBets.Enqueue(bet);

                return;
            }

            bet.SetDateArrived(_dateTimeProvider.GetUTCNow());
            _betAgregator.AddBet(bet);
        }

        public async Task WhenAllHandled()
        {
            await _workersDirector.WhenAllBetsCalculated();
        }

        public void ShutDown()
        {
            isShuttingDown = true;

            _workersDirector.ShutDown();

            _betAgregator.ShutDown();

            var betsCalculated = _workersDirector.GetBetsCalculatedSnapshot();

            _betRepository.Save(betsCalculated);

            
            // todo: save unhandled bets
        }

        public BetCalculated[] GetBets()
        {
            return betsCalculated.ToArray();
        }
    }
}
