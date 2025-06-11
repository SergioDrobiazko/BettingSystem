using System;

namespace BettingCompany.BettingSystem.Domain
{
    public class BetCalculatedEventArgs : EventArgs
    {
        public int BetId { get; init; }
    }
}
