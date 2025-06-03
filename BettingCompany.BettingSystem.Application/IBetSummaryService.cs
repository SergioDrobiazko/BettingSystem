using BettingCompany.BettingSystem.Domain;

namespace BettingCompany.BettingSystem.Application
{
    public interface IBetSummaryService
    {
        BetSummary GetSummary();
    }
}