using BettingCompany.BettingSystem.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Application
{
    public interface IBetHandlingService
    {
        void Handle(Bet bet);
        Task WhenAllHandled();
    }
}