namespace OnlineBankingSystem.Models
{
    public class CustomerAccount : Account
    {
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public CustomerAccount() : base()
        {
            Console.WriteLine("Customer account created");
        }

        public CustomerAccount(int accountNumber, string accountHolderName, string accountType, double balance) 
            : base(accountNumber, accountHolderName, accountType, balance)
        {
            Console.WriteLine("Customer account created with parameters");
        }

        public override void DisplayAccountInfo()
        {
            Console.WriteLine($"Account No: {AccountNumber}, Holder: {AccountHolderName}, Balance: {Balance}");
        }

        public void AddTransaction(string description, double amount, string transactionType)
        {
            Transactions.Add(new Transaction(DateTime.Now, description, amount, transactionType));
        }
    }
}
