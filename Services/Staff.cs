using OnlineBankingSystem.Interfaces;
using OnlineBankingSystem.Models;
using System;

namespace OnlineBankingSystem.Services
{
    public class Staff : IRecordSearch
    {
        private List<CustomerAccount> _accounts;

        public Staff(List<CustomerAccount> accounts)
        {
            _accounts = accounts;
        }
        public void EnterRecord(CustomerAccount account)
        {
            _accounts.Add(account);
            Console.WriteLine("Record entered by staff.");
        }
        public void SearchRecord(int accountNumber)
        {
            var found = _accounts.Find(a => a.AccountNumber == accountNumber);
            if (found != null)
            {
                Console.WriteLine($"Found: {found.AccountHolderName}, Balance: {found.Balance}");
            }
            else
                Console.WriteLine("Record not found.");
        }
    }
}
