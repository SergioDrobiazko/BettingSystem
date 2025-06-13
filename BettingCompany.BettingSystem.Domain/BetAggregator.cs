using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class BetAggregator : IBetAggregator
    {
        private bool isShuttingDown = false;

        private readonly ConcurrentDictionary<int, Bet> registeredBets = new ();

        private readonly ConcurrentQueue<BetTransition> betTransitions = new ();

        public event EventHandler<BetTransitionFormedEventArgs> BetTransitionFormed;

        public void AddBet(Bet bet)
        {
            if(isShuttingDown)
            {
                return;
            }

            if(IsRegistered(bet))
            {
                var registeredBet = registeredBets[bet.Id];
                var betTransition = new BetTransition(registeredBet, bet);

                betTransitions.Enqueue(betTransition);
                betTransitions.TryDequeue(out BetTransition firstBetTransition);
                registeredBets.TryRemove(bet.Id, out _);
                BetTransitionFormed?.Invoke(this, new BetTransitionFormedEventArgs { BetTransition = firstBetTransition });
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

        public void ShutDown()
        {
            isShuttingDown = true;
        }
    }
}
