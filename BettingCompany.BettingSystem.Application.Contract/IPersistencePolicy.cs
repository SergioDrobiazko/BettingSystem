namespace BettingCompany.BettingSystem.Application.Contract
{
    public interface IPersistencePolicy
    {
        int GetNumberOfElementsToSave();
        bool ShouldPersist(int betsCalculated, int betsSaved);
    }
}
