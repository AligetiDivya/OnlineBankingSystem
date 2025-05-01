using OnlineBankingSystem.Models;

namespace OnlineBankingSystem.Services
{
    public static class BankSystem
    {
        static List<CustomerAccount> accounts = new List<CustomerAccount>();
        static Staff staff;

        static BankSystem()
        {
            Console.WriteLine("Welcome to Online Banking System!");
            staff = new Staff(accounts);
        }

        public static void Start()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("1. Create Account\n2. Balance Check\n3. View Transaction History\n4. Transfer Balance\n5. Staff Data Entry\n6. Search Record\n7. Request Checkbook\n8. Exit");
                Console.Write("Select option: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        CreateAccount();
                        break;
                    case 2:
                        BalanceCheck();
                        break;
                    case 3:
                        ViewTransactionHistory();
                        break;
                    case 4:
                        TransferBalance();
                        break;
                    case 5:
                        StaffDataEntry();
                        break;
                    case 6:
                        SearchRecord();
                        break;
                    case 7:
                        RequestCheckBook();
                        break;
                    case 8:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid Option!");
                        break;
                }
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }

            Console.WriteLine("Thank you for using the Online Banking System.");
        }

        static CustomerAccount? FindAccount(int accountNumbers)
        {
            return accounts.Find(a => a.AccountNumber == accountNumbers);
        }

        static void CreateAccount()
        {
            Console.Write("Enter Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }
            Console.Write("Enter Account Holder Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Account Type: ");
            string accountType = Console.ReadLine();
            Console.Write("Enter Initial Balance: ");
            if (!double.TryParse(Console.ReadLine(), out double balance))
            {
                Console.WriteLine("Invalid balance amount.");
                return;
            }

            CustomerAccount newAccount = new CustomerAccount(accountNumber, name, accountType, balance); 
            accounts.Add(newAccount);
            Console.WriteLine("Account Created Successfully.");
        }

        static void BalanceCheck()
        {
            Console.Write("Enter Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }
            var account = FindAccount(accountNumber);
            if (account != null)
                account.DisplayAccountInfo();
            else
                Console.WriteLine("Account not found.");
        }

        static void ViewTransactionHistory()
        {
            Console.Write("Enter Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }
            var account = FindAccount(accountNumber);
            if (account != null)
            {
                var transactions = account.Transactions.FindAll(t => (DateTime.Now - t.TransactionDate).TotalDays <= 365);
                if (transactions.Count == 0)
                {
                    Console.WriteLine("No recent transactions");
                    return;
                }
                Console.WriteLine("Transaction History:");
                foreach (var t in transactions)
                {
                        Console.WriteLine($"{t.TransactionDate.ToShortDateString()} - {t.Description} - {t.Amount} - {t.TransactionType}");
                }
            }
            else
                Console.WriteLine("Account not found.");
        }

        static void TransferBalance()
        {
            Console.Write("Enter Sender Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int senderNo))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }
            Console.Write("Enter Receiver Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int receiverNo))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }
            Console.Write("Enter Amount: ");
            if (!double.TryParse(Console.ReadLine(), out double amount))
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            var sender = FindAccount(senderNo);
            var receiver = FindAccount(receiverNo);

            if (sender != null && receiver != null)
            {
                if(sender.Balance >= amount)
                {
                    sender.Balance -= amount;
                    receiver.Balance += amount;

                    sender.AddTransaction("Transfer to " + receiver.AccountHolderName, amount, "Debit");
                    receiver.AddTransaction("Transfer from " + sender.AccountHolderName, amount, "Credit");
                    Console.WriteLine("Transfer Successful.");
                }
            }
            else
            {
                Console.WriteLine("Transfer Failed. Check account numbers.");
            }
        }

        static void StaffDataEntry()
        {
            Console.Write("Enter New Customer Account Number: ");
            if (!int.TryParse(Console.ReadLine(), out int newAccNo))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }
            Console.Write("Enter New Customer Name: ");
            string custName = Console.ReadLine();
            Console.Write("Enter Account Type: ");
            string accType = Console.ReadLine(); 
            Console.Write("Enter Opening Balance: ");
            if (!double.TryParse(Console.ReadLine(), out double custBalance))
            {
                Console.WriteLine("Invalid balance amount.");
                return;
            }
            CustomerAccount customerAccount = new CustomerAccount
            {
                AccountNumber = newAccNo,
                AccountHolderName = custName,
                AccountType = accType,
                Balance = custBalance
            };
            staff.EnterRecord(customerAccount);
        }

        static void SearchRecord()
        {
            Console.Write("Enter Account Number to Search: ");
            if (!int.TryParse(Console.ReadLine(), out int searchAccNo))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            staff.SearchRecord(searchAccNo);
        }

        static void RequestCheckBook()
        {
            Console.Write("Enter Account Number for Checkbook Request: ");
            if (!int.TryParse(Console.ReadLine(), out int checkAccNo))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            var checkAccount = FindAccount(checkAccNo); 
            if (checkAccount != null)
                CheckBookRequest.Request(checkAccount);
            else
                Console.WriteLine("Account not found.");
        }
    }
}
