using BettingCompany.BettingSystem.Application.Contract;
using BettingCompany.BettingSystem.Domain;
using BettingCompany.BettingSystem.Repository;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application
{
    public class BetHandlingService : IBetHandlingService
    {
        private bool isShuttingDown = false;

        private readonly IBetAgregator _betAgregator;
        private readonly IWorkersDirector _workersDirector;
        private readonly IPersistencePolicy persistencePolicy;
        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly IBetRepository _betRepository;

        private readonly ILogger<BetHandlingService> _logger;

        private readonly ConcurrentQueue<BetCalculated> betsCalculated = new();

        private int incomingBets = 0;
        private int betsHandled = 0;
        private int betsSaved = 0;

        private readonly object betsCalculatedCounterLock = new();
        private readonly object incomingBetsLock = new();

        public BetHandlingService(
            IBetAgregator betAgregator,
            IWorkersDirector workersDirector,
            IPersistencePolicy persistencePolicy,
            IDateTimeProvider dateTimeProvider,
            IBetRepository betRepository, 
            ILogger<BetHandlingService> logger)
        {
            _betAgregator = betAgregator;
            _betAgregator.BetTransitionFormed += OnBetTransitionFormed;

            _workersDirector = workersDirector;
            _workersDirector.BetCalculated += OnBetCalculated;
            this.persistencePolicy = persistencePolicy;
            _dateTimeProvider = dateTimeProvider;
            _betRepository = betRepository;
            _logger = logger;
        }

        private void OnBetTransitionFormed(object sender, BetTransitionFormedEventArgs e)
        {
            _workersDirector.DelegateBetCalculation(e.BetTransition);
        }

        private void OnBetCalculated(object sender, BetCalculatedEventArgs e)
        {
            _logger.LogDebug($"Bet calculated. Bet id = {e.BetId}");

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
                if (!persistencePolicy.ShouldPersist(betsHandled, betsSaved)) return;
                _logger.LogDebug($"Saving bets..");

                int elementsToSave = persistencePolicy.GetNumberOfElementsToSave();

                var betsToSave = new BetCalculated[elementsToSave];

                for (int i = 0; i < elementsToSave; ++i)
                {
                    betsCalculated.TryDequeue(out var betCalculated);
                    betsToSave[i] = betCalculated;
                }
                _betRepository.Save(betsToSave);
                betsSaved += elementsToSave;

                _logger.LogDebug($"Saved {elementsToSave} bets");
            }
        }

        private readonly ConcurrentQueue<Bet> unhandledBets = new();

        public BetCalculated[] GetBetsSnapshot()
        {
            return betsCalculated.ToArray();
        }

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
            _logger.LogDebug($"{nameof(BetHandlingService)} is shutting down");

            isShuttingDown = true;

            _workersDirector.ShutDown();

            _betAgregator.ShutDown();

            var betsToSave = betsCalculated.ToArray();

            if (!betsToSave.Any()) return;
            _betRepository.Save(betsCalculated.ToArray());

            _logger.LogDebug($"Saved {betsCalculated.Count} calculated bets");

            // todo: save unhandled bets
        }
    }
}
