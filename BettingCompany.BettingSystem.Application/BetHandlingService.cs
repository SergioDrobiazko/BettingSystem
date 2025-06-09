using BettingCompany.BettingSystem.Domain;
using BettingCompany.BettingSystem.Repository;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application
{
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

            lock (StorageLock.Lock)
            {
                if (_persistancePolicy.ShouldPersist(betsHandled, betsSaved))
                {
                    int elementsToSave = _persistancePolicy.GetNumberOfElementsToSave();

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

        public async Task HandleAsync(Bet bet)
        {
            await Task.Run(() =>
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
            });
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

            var betsToSave = betsCalculated.ToArray();

            if (betsToSave.Any())
            {
                _betRepository.Save(betsCalculated.ToArray());
            }

            // todo: save unhandled bets
        }
    }
}
