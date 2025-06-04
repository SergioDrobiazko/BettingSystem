using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettingCompany.BettingSystem.Domain
{
    public class Bet
    {
        public Bet(int id, double amount, double odds, string client, string @event, string market, string selection, BetStatus status)
        {
            Id = id;
            Amount = amount;
            Odds = odds;
            Client = client;
            Event = @event;
            Market = market;
            Selection = selection;
            Status = status;
        }

        public int Id { get; private set; }
        public double Amount { get; private set; }
        public double Odds { get; private set; }
        public string Client { get; private set; }
        public string Event { get; private set; }
        public string Market { get; private set; }
        public string Selection { get; private set; }
        public BetStatus Status { get; private set; }

        public DateTime ArrivedUTC { get; private set; }

        public void SetDateArrived(DateTime arrivedUTC)
        {
            ArrivedUTC = arrivedUTC;
        }
    }
}
