namespace OnlineBankingSystem.Models
{
    public class Transaction
    {
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string TransactionType { get; set; }

        public Transaction(DateTime transactionDate, string description, double amount, string transactionType)
        {
            TransactionDate = transactionDate;
            Description = description;
            Amount = amount;
            TransactionType = transactionType;
        }
    }
}