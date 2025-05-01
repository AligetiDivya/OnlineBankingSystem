using OnlineBankingSystem.Models;
using System;

namespace OnlineBankingSystem.Services
{
    public class CheckBookRequest
    {
        public static void Request(CustomerAccount account)
        {
            Console.WriteLine($"Checkbook requested for account {account.AccountNumber}");
        }
    }
}
