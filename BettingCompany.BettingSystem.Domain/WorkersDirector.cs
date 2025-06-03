using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class ChunkReadyEventArgs : EventArgs
    {
        public ConcurrentQueue<BetCalculated> BetsCalculated { get; init; }
    }

    public class WorkersDirector : IWorkersDirector
    {
        public WorkersDirector(int maxWorkers)
        {
            workers = new List<IWorker>(maxWorkers);
        }

        private IList<IWorker> workers = new List<IWorker>();

        private ConcurrentQueue<BetCalculated> betsCalculated = new ();

        private int chunkSize = 1000;

        public event EventHandler<ChunkReadyEventArgs> ChunkReady;

        public void DelegateWork(IBetAgregator betAgregator)
        {
            IWorker freeWorker = GetFreeWorker();
            var betCalculatedTask = freeWorker.CalculateBet();

            betCalculatedTask
                .ContinueWith((betCalculated) =>
                {
                    betsCalculated.Enqueue(betCalculated.Result);
                    if(betsCalculated.Count > chunkSize)
                    {
                        ChunkReady?.Invoke(this, new ChunkReadyEventArgs { BetsCalculated = betsCalculated });
                        betsCalculated = new ConcurrentQueue<BetCalculated>();
                    }
                });
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
