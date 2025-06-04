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

        public int Id { get; set; }
        public double Amount { get; set; }
        public double Odds { get; set; }
        public string Client { get; set; }
        public string Event { get; set; }
        public string Market { get; set; }
        public string Selection { get; set; }
        public BetStatus Status { get; set; }

        public DateTime ArrivedUTC { get; set; }

        public void SetDateArrived(DateTime arrivedUTC)
        {
            ArrivedUTC = arrivedUTC;
        }
    }
}
