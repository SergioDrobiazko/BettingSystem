using BettingCompany.BettingSystem.Domain;
using System.Collections.Concurrent;

namespace BettingCompany.BettingSystem.Application
{
    public interface IPersistancePolicy
    {
        int GetNumberOfElementsToSave();
        bool ShouldPersist(int betsCalculated);
    }
}