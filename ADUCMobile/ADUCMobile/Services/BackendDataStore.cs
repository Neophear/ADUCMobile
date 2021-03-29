using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ADUCMobile.Controllers;
using ADUCMobile.Helpers;
using ADUCMobile.Interfaces;
using ADUCMobile.Models;
using ADUCMobile.Models.Exceptions;
using Newtonsoft.Json;

namespace ADUCMobile.Services
{
    class BackendDataStore : IDataStore<Account>
    {
        ICommunicator communicator;

        public BackendDataStore()
        {
            communicator = AppSettings.GetInstance().Communicator;
        }

        public async Task<Account> GetAccountAsync(string accountName)
        {
            if (accountName == null)
                throw new NotFoundException();
            
            string response = await communicator.Get($"{App.ADUCBackendUrl}/api/account/{accountName}", await UserController.GetInstance().GetToken());
            return JsonConvert.DeserializeObject<Account>(response);
        }

        public async Task UpdateAccountAsync(Account account)
        {
            if (account?.AccountName == null)
                throw new NotFoundException();

            var serializedAccount = JsonConvert.SerializeObject(account);
            await communicator.Post($"{App.ADUCBackendUrl}/api/account", serializedAccount, await UserController.GetInstance().GetToken());
        }

        public async Task<bool> CheckConnection()
        {
            await communicator.Get($"{App.ADUCBackendUrl}/api/ping");
            return true;
        }
    }
}