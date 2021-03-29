using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADUCMobile.Services
{
    public interface IDataStore<T>
    {
        Task UpdateAccountAsync(T account);
        Task<T> GetAccountAsync(string accountName);
        Task<bool> CheckConnection();
    }
}