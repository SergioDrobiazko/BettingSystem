using System;

namespace BettingCompany.BettingSystem.Domain
{
    public class WorkersFactory : IWorkersFactory
    {
        public IWorker CreateWorker()
        {
            return new Worker();
        }
    }
}