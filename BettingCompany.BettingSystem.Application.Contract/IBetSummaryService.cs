using BettingCompany.BettingSystem.Domain;

namespace BettingCompany.BettingSystem.Application.Contract
{
    public interface IBetSummaryService
    {
        BetSummary GetSummary();
    }
}
