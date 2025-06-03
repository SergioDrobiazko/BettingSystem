using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class BetAgregator : IBetAgregator
    {
        private Dictionary<int, Bet> registeredBets = new Dictionary<int, Bet>();

        private Queue<BetTransitions> betTransitions = new Queue<BetTransitions>();

        public void AddBet(Bet bet)
        {

        }

        public IEnumerable<BetTransitions> GetBetTransitions()
        {
            return betTransitions;
        }
    }
}
