using BettingCompany.BettingSystem.Domain;
using System.Collections.Concurrent;

namespace BettingCompany.BettingSystem.Application
{
    internal interface IPersistancePolicy
    {
        bool ShouldPersist(ConcurrentQueue<BetCalculated> betsCalculated);
    }
}