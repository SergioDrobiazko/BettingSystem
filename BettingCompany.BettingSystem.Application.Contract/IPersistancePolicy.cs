namespace BettingCompany.BettingSystem.Application.Contract
{
    public interface IPersistancePolicy
    {
        int GetNumberOfElementsToSave();
        bool ShouldPersist(int betsCalculated, int betsSaved);
    }
}
