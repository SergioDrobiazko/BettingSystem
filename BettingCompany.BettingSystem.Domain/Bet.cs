using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class Bet
    {
        public int Id { get; private set; }
        public double Amount { get; private set; }
        public double Odds { get; private set; }
        public string Client { get; private set; }
        public string Event { get; private set; }
        public string Market { get; private set; }
        public string Selection { get; set; }
        public BetStatus Status { get; private set; }
    }
}
