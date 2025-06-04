using System;

namespace BettingCompany.BettingSystem.Application
{
    public interface IDateTimeProvider
    {
        DateTime GetUTCNow();
    }
}