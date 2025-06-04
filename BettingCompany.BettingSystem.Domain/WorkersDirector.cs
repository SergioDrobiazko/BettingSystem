using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class BetCalculatedEventArgs : EventArgs
    {
        public int BetCalculatedId { get; init; }
    }

    public class WorkersDirector : IWorkersDirector
    {
        public WorkersDirector(int maxWorkers)
        {
            workers = new List<IWorker>(maxWorkers);
        }

        private IList<IWorker> workers = new List<IWorker>();

        private ConcurrentQueue<BetCalculated> betsCalculated = new ();

        public event EventHandler<BetCalculatedEventArgs> BetCalculated;

        public void DelegateWork(BetTransition betTransition)
        {
            IWorker freeWorker = GetFreeWorker();
            var betCalculatedTask = freeWorker.CalculateBet(betTransition);

            betCalculatedTask
                .ContinueWith((betCalculated) =>
                {
                    betsCalculated.Enqueue(betCalculated.Result);
                    
                    BetCalculated?.Invoke(this, new BetCalculatedEventArgs { BetCalculatedId = betCalculated.Result.Id });
                });
        }

        public BetCalculated? FetchCalculatedBet()
        {
            var calculatedBet = betsCalculated.TryDequeue(out BetCalculated betCalculated) ? betCalculated : null;
            return calculatedBet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IWorker GetFreeWorker()
        {
            throw new NotImplementedException();
        }
    }
}
