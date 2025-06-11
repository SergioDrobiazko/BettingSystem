using System;

namespace BettingCompany.BettingSystem.Application.Contract
{
    public interface IDateTimeProvider
    {
        DateTime GetUTCNow();
    }
}
