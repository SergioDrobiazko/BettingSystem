using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class BetCalculated
    {
        public Bet Bet { get; set; }

        public BetOutcome BetOutcome { get; private set; }

        public bool IsMarkedForReview { get; private set; }
    }
}
