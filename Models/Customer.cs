using OnlineBankingSystem.Models;

namespace OnlineBankingSystem.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public bool CheckbookRequested { get; set; } = false;

    }
}
