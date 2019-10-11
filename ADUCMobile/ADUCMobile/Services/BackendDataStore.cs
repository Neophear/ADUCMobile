using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ADUCMobile.Controllers;
using ADUCMobile.Models;
using Newtonsoft.Json;

namespace ADUCMobile.Services
{
    class BackendDataStore : IDataStore<Account>
    {
        HttpClient client;
        IEnumerable<Account> accounts;

        public BackendDataStore()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri($"{App.ADUCBackendUrl}/")
            };

            string token = UserController.GetInstance().GetToken();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            accounts = new List<Account>();
        }

        public async Task<bool> AddAccountAsync(Account account)
        {
            if (account == null)
                return false;

            var serializedAccount = JsonConvert.SerializeObject(account);
            var response = await client.PostAsync($"api/account", new StringContent(serializedAccount, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAccountAsync(string accountName)
        {
            if (string.IsNullOrEmpty(accountName))
                return false;

            var response = await client.DeleteAsync($"api/account/{accountName}");

            return response.IsSuccessStatusCode;
        }

        public async Task<Account> GetAccountAsync(string accountName)
        {
            if (accountName != null)
            {
                HttpResponseMessage response = await client.GetAsync($"api/account/{accountName}");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return await Task.Run(() => JsonConvert.DeserializeObject<Account>(json));
                }
                else
                    return null;
            }

            return null;
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync(bool forceRefresh = false)
        {
            if (forceRefresh)
            {
                var json = await client.GetStringAsync($"api/account");
                accounts = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Account>>(json));
            }

            return accounts;
        }

        public async Task<bool> UpdateAccountAsync(Account account)
        {
            if (account == null || account.AccountName == null)
                return false;

            var serializedAccount = JsonConvert.SerializeObject(account);
            var buffer = Encoding.UTF8.GetBytes(serializedAccount);
            var byteContent = new ByteArrayContent(buffer);

            var response = await client.PutAsync(new Uri($"api/account/{account.AccountName}"), byteContent);

            return response.IsSuccessStatusCode;
        }
    }
}
