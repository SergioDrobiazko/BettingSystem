using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application.Contract
{
    public interface IPersistancePolicy
    {
        int GetNumberOfElementsToSave();
        bool ShouldPersist(int betsCalculated, int betsSaved);
    }
}
