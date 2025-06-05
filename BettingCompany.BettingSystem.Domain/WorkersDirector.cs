using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class WorkersDirector : IWorkersDirector
    {
        public WorkersDirector(int maxWorkers)
        {
            for(int i = 0; i < maxWorkers; ++i)
            {
                availableWorkers.Add(new Worker());
            }
        }

        private BlockingCollection<IWorker> availableWorkers = new BlockingCollection<IWorker>(new ConcurrentQueue<IWorker>());

        private ConcurrentQueue<BetCalculated> betsCalculated = new ();

        public event EventHandler<BetCalculatedEventArgs> BetCalculated;

        public void DelegateBetCalculation(BetTransition betTransition)
        {
            IWorker freeWorker = GetFreeWorker();
            var betCalculatedTask = freeWorker.CalculateBetAsync(betTransition);

            betCalculatedTask
                .ContinueWith((betCalculated) =>
                {
                    betsCalculated.Enqueue(betCalculated.Result);
                    
                    BetCalculated?.Invoke(this, new BetCalculatedEventArgs { });

                    availableWorkers.Add(freeWorker);
                });
        }

        public BetCalculated? FetchCalculatedBet()
        {
            var calculatedBet = betsCalculated.TryDequeue(out BetCalculated betCalculated) ? betCalculated : null;
            return calculatedBet;
        }

        public BetCalculated[] CopyBetsCalculated()
        {
            return betsCalculated.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IWorker GetFreeWorker()
        {
            return availableWorkers.Take();
        }
    }
}
