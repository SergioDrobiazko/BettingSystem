using BettingCompany.BettingSystem.Application.Contract;

namespace BettingCompany.BettingSystem.Application
{
    public class PersistencePolicy : IPersistencePolicy
    {
        public int GetNumberOfElementsToSave()
        {
            return 20;
        }

        public bool ShouldPersist(int betsCalculated, int betsSaved)
        {
            return betsCalculated - betsSaved >= 20; 
        }
    }
}
