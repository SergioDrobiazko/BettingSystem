using BettingCompany.BettingSystem.Application.Contract;
using System;

namespace BettingCompany.BettingSystem.Application
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUTCNow()
        {
            return DateTime.UtcNow;
        }
    }
}
