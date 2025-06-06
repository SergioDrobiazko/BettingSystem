using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class WorkersDirector : IWorkersDirector
    {
        private bool isShuttingDown = false;

        public WorkersDirector(int maxWorkers, IWorkersFactory workersFactory)
        {
            for (int i = 0; i < maxWorkers; ++i)
            {
                availableWorkers.Add(workersFactory.CreateWorker());
            }
        }

        private BlockingCollection<IWorker> availableWorkers = new(new ConcurrentQueue<IWorker>());

        private ConcurrentQueue<BetCalculated> betsCalculated = new();

        private ConcurrentQueue<Task> betsCalculationTasks = new();

        public event EventHandler<BetCalculatedEventArgs> BetCalculated;

        private readonly CancellationTokenSource Cts = new();

        public void CancelOperations()
        {
            Cts.Cancel();
        }

        private ConcurrentQueue<BetTransition> unhandledTransitions = new();

        public void DelegateBetCalculation(BetTransition betTransition)
        {
            if(isShuttingDown == true)
            {
                unhandledTransitions.Enqueue(betTransition);
                return;
            }

            IWorker freeWorker = GetFreeWorker();

            var cancellationToken = Cts.Token;

            var betCalculatedTask = freeWorker.CalculateBetAsync(betTransition, cancellationToken)
                .ContinueWith((betCalculated) =>
                {
                    if (betCalculated.IsFaulted)
                    {
                        Console.WriteLine("Task failed: " + betCalculated.Exception);
                        return;
                    }

                    if (betCalculated.IsCanceled)
                    {
                        Console.WriteLine("Task was canceled.");
                        return;
                    }

                    betsCalculated.Enqueue(betCalculated.Result);
                    BetCalculated?.Invoke(this, new BetCalculatedEventArgs { });
                    availableWorkers.Add(freeWorker);
                }, cancellationToken);

            betsCalculationTasks.Enqueue(betCalculatedTask);
        }

        public BetCalculated? FetchCalculatedBet()
        {
            var calculatedBet = betsCalculated.TryDequeue(out BetCalculated betCalculated) ? betCalculated : null;
            return calculatedBet;
        }

        public BetCalculated[] GetBetsCalculatedSnapshot()
        {
            return betsCalculated.ToArray();
        }

        public async Task WhenAllBetsCalculated()
        {
            await Task.WhenAll(betsCalculationTasks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IWorker GetFreeWorker()
        {
            return availableWorkers.Take();
        }

        public void ShutDown()
        {
            isShuttingDown = true;

            CancelOperations();
        }
    }
}
