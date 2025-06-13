using System;

namespace BettingCompany.BettingSystem.Domain;

public class BetTransitionFormedEventArgs : EventArgs 
{
    public BetTransition BetTransition { get; init; }
}