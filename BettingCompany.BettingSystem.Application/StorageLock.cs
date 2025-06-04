using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application
{
    public static class StorageLock
    {
        public static readonly SemaphoreSlim Lock = new SemaphoreSlim(1, 1);
    }
}
