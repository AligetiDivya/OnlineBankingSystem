using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBankingSystem.Models
{
    //Base class for customer account and abstract class cannot be instantiated
    public abstract class Account
    {
        public int AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountType { get; set; }
        public double Balance { get; set; }

        public Account()
        {
            Console.WriteLine("Account created");
        }
        public Account(int accountNumber, string accountHolderName, string accountType, double balance)
        {
            AccountNumber = accountNumber;
            AccountHolderName = accountHolderName;
            AccountType = accountType;
            Balance = balance;
        }
        public abstract void DisplayAccountInfo();
    }
}
