using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADUCMobile.Models;

namespace ADUCMobile.Services
{
    public class MockDataStore : IDataStore<Account>
    {
        List<Account> accounts;

        public MockDataStore()
        {
            accounts = new List<Account>();
            var mockAccounts = new List<Account>
            {
                new Account{AccountName = "Account1", FirstName="FN1", LastName="LN1"},
                new Account{AccountName = "Account2", FirstName="FN2", LastName="LN2"},
                new Account{AccountName = "Account3", FirstName="FN3", LastName="LN3"}
            };

            foreach (var account in mockAccounts)
            {
                accounts.Add(account);
            }
        }

        public async Task<bool> AddAccountAsync(Account account)
        {
            accounts.Add(account);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            var oldAccount = accounts.Where((Account arg) => arg.AccountName == account.AccountName).FirstOrDefault();
            accounts.Remove(oldAccount);
            accounts.Add(account);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAccountAsync(string accountName)
        {
            var oldAccount = accounts.Where((Account arg) => arg.AccountName == accountName).FirstOrDefault();
            accounts.Remove(oldAccount);

            return await Task.FromResult(true);
        }

        public async Task<Account> GetAccountAsync(string accountName)
        {
            return await Task.FromResult(accounts.FirstOrDefault(s => s.AccountName == accountName));
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(accounts);
        }
    }
}