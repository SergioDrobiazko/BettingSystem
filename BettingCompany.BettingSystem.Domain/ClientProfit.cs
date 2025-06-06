namespace BettingCompany.BettingSystem.Domain
{
    public class ClientProfit
    {
        public ClientProfit(string client, decimal profit)
        {
            Client = client;
            Profit = profit;
        }

        public string Client { get; set; }
        public decimal Profit { get; set; }
    }
}
