using System;

namespace BettingCompany.BettingSystem.Application
{
    internal interface IDateTimeProvider
    {
        DateTime GetUTCNow();
    }
}