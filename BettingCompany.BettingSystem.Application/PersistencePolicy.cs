using BettingCompany.BettingSystem.Application.Contract;

namespace BettingCompany.BettingSystem.Application
{
    public class PersistencePolicy : IPersistencePolicy
    {
        private const int ElementsToSave = 20;
        
        public int GetNumberOfElementsToSave()
        {
            return ElementsToSave;
        }

        public bool ShouldPersist(int betsCalculated, int betsSaved)
        {
            return betsCalculated - betsSaved >= ElementsToSave; 
        }
    }
}
