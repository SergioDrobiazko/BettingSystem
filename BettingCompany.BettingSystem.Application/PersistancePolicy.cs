namespace BettingCompany.BettingSystem.Application
{
    public class PersistancePolicy : IPersistancePolicy
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
