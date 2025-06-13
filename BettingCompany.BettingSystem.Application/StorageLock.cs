using System.Threading;

namespace BettingCompany.BettingSystem.Application
{
    public static class StorageLock
    {
        public static readonly SemaphoreSlim Lock = new(1, 1);
    }
}
