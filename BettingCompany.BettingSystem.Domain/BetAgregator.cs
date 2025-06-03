using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class BetTransitionFormedEventArgs : EventArgs 
    {

    }

    public class BetAgregator : IBetAgregator
    {
        private ConcurrentDictionary<int, Bet> registeredBets = new ();

        private ConcurrentQueue<BetTransition> betTransitions = new ();

        public event EventHandler<BetTransitionFormedEventArgs> BetTransitionFormed;

        public void AddBet(Bet bet)
        {
            if(IsRegistered(bet))
            {
                var registeredBet = registeredBets[bet.Id];
                var betTransition = new BetTransition(registeredBet, bet);
                betTransitions.Enqueue(betTransition);
            }
            else
            {
                RegisterBet(bet);
            }
        }

        private void RegisterBet(Bet bet)
        {
            registeredBets[bet.Id] = bet;
        }

        private bool IsRegistered(Bet bet)
        {
            return registeredBets.ContainsKey(bet.Id);
        }

        public bool TryGetBetTransition(out BetTransition betTransition)
        {
            return betTransitions.TryDequeue(out betTransition);
        }
    }
}
