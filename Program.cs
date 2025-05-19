using OnlineBankingSystem.Models;

namespace OnlineBankingSystem
{
    class Program
    {
        static List<Customer> customers = new List<Customer>();

        static void Main(string[] args)
        {
            Console.WriteLine("=== Online Banking System===");

            while (true)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Create New Bank Account");
                Console.WriteLine("2. Check Balance");
                Console.WriteLine("3. Balance Transfer");
                Console.WriteLine("4. View Transaction History");
                Console.WriteLine("5. Search Customer Record");
                Console.WriteLine("6. Request Checkbook");
                Console.WriteLine("7. Staff: Add Customer Record");
                Console.WriteLine("8. Exit");

                Console.WriteLine("Choose an option from above");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateAccount();
                        break;
                    case "2":
                        CheckBalance();
                        break;
                    case "3":
                        TransferBalance();
                        break;
                    case "4":
                        ViewTransactions();
                        break;
                    case "5":
                        SearchCustomer();
                        break;
                    case "6":
                        RequestCheckbook();
                        break;
                    case "7":
                        StaffCreateCustomer();
                        break;
                    case "8":
                        Console.WriteLine("Exiting Application!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static void CreateAccount()
        {
            Console.WriteLine("=== Create New Bank Account===");

            Console.Write("Enter Customer ID:");
            string input = Console.ReadLine();
            if (!int.TryParse(input, out int customerId))
            {
                Console.WriteLine("Invalid input. Must be a Number.");
                return;
            }
            if (customers.Any(c => c.CustomerId == customerId))
            {
                Console.Write("Customer ID already exists.");
                return;
            }
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name) || !System.Text.RegularExpressions.Regex.IsMatch(name, @"^[A-Za-z\s]+$"))
            {
                Console.WriteLine("Invalid name. Only letters and spaces are allowed.");
                return;
            }
            Console.Write("Enter Initial Balance: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal balance))
            {
                Console.WriteLine("Invalid balance.");
                return;
            }
            customers.Add(new Customer
            {
                CustomerId = customerId,
                Name = name,
                Balance = balance
            });
            Console.WriteLine($"Account created successfully for {name} with ID {customerId} and balance {balance}");
        }

        static void CheckBalance()
        {
            Console.WriteLine("=== Balance Check ===");

            Console.Write("Enter your Customer ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = customers.FirstOrDefault(c => c.CustomerId == id);

                if (customer != null)
                {
                    Console.WriteLine($"Hello {customer.Name}, your balance is ${customer.Balance}");
                }
                else
                {
                    Console.WriteLine("Customer not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        static void TransferBalance()
        {
            Console.WriteLine(" === Balance Transfer === ");

            Console.WriteLine("Enter Sender Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int senderId))
            {
                Console.WriteLine("Invalid ID for sender.");
                return;
            }

            var sender = customers.FirstOrDefault(c => c.CustomerId == senderId);
            if (sender == null)
            {
                Console.WriteLine("Sender account not found.");
                return;
            }

            Console.WriteLine("Enter Receiver Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int receiverId))
            {
                Console.WriteLine("Invalid ID for receiver.");
                return;
            }

            if (receiverId == senderId)
            {
                Console.WriteLine("Cannot transfer to the same account.");
                return;
            }

            var receiver = customers.FirstOrDefault(c => c.CustomerId == receiverId);
            if (receiver == null)
            {
                Console.WriteLine("Receiver account not found.");
                return;
            }

            Console.Write("Enter amount to transfer: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid amount.");
                return;
            }

            if (sender.Balance < amount)
            {
                Console.WriteLine("Insufficient balance.");
                return;
            }

            sender.Balance -= amount;
            receiver.Balance += amount;

            Console.WriteLine($"Successfully transferred ${amount} from {sender.Name} to {receiver.Name}.");
            Console.WriteLine($"{sender.Name}'s new balance: ${sender.Balance}");
            Console.WriteLine($"{receiver.Name}'s new balance: ${receiver.Balance}");

            //Log the transaction for sender
            sender.Transactions.Add(new Transaction
            {
                Date = DateTime.Now,
                Type = "Debit",
                Amount = amount,
                Description = $"Transferred to {receiver.Name} (ID: {receiver.CustomerId})"
            });

            //Log the transaction for receiver
            receiver.Transactions.Add(new Transaction
            {
                Date = DateTime.Now,
                Type = "Credit",
                Amount = amount,
                Description = $"Received from {sender.Name} (ID: {sender.CustomerId})"
            });
        }

        static void ViewTransactions()
        {
            Console.WriteLine("=== Transaction History (Last 1 Year) ===");
            Console.Write("Enter your Customer ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = customers.FirstOrDefault(c => c.CustomerId == id);
                if (customer != null)
                {
                    var oneYearAgo = DateTime.Now.AddYears(-1);
                    var transactions = customer.Transactions
                        .Where(t => t.Date >= oneYearAgo)
                        .OrderByDescending(t => t.Date)
                        .ToList();
                    if (transactions.Any())
                    {
                        Console.WriteLine($"Transaction history for {customer.Name}:");
                        foreach (var transaction in transactions)
                        {
                            Console.WriteLine($"{transaction.Date}: {transaction.Type} ${transaction.Amount} - {transaction.Description}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No transactions found in the last year.");
                    }
                }
                else
                {
                    Console.WriteLine("Customer not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        static void SearchCustomer()
        {
            Console.WriteLine("\n=== Search Customer Record ===");
            Console.Write("Enter ID or Name (partial match allowed): ");
            string input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input cannot be empty.");
                return;
            }

            var results = customers.Where(c =>
                c.CustomerId.ToString().StartsWith(input) ||     // ID starts with input
                c.Name.ToLower().Contains(input))                // Name contains input
                .ToList();

            if (results.Any())
            {
                Console.WriteLine("Matching Customers:");
                foreach (var c in results)
                {
                    Console.WriteLine($"ID: {c.CustomerId}, Name: {c.Name}, Balance: ${c.Balance}");
                }
            }
            else
            {
                Console.WriteLine("No matching customer records found.");
            }
        }


        static void RequestCheckbook()
        {
            Console.WriteLine("=== Request Checkbook ===");
            Console.Write("Enter your Customer ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var customer = customers.FirstOrDefault(c => c.CustomerId == id);
                if (customer != null)
                {
                    if (customer.CheckbookRequested)
                    {
                        Console.WriteLine("You already requested a checkbook.");
                    }
                    else
                    {
                        customer.CheckbookRequested = true;
                        Console.WriteLine("Checkbook request submitted successfully.");
                    }
                }
                else
                {
                    Console.WriteLine("Customer not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }


        static void StaffCreateCustomer()
        {
            Console.WriteLine("=== Staff Access: Add Customer Record ===");
            Console.Write("Enter Staff Password: ");
            string password = Console.ReadLine();

            if (password != "admin123")
            {
                Console.WriteLine("Access denied. Invalid password.");
                return;
            }

            CreateAccount();
        }

    }
}
