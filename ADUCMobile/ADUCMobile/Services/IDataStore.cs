using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ADUCMobile.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddAccountAsync(T account);
        Task<bool> UpdateAccountAsync(T account);
        Task<bool> DeleteAccountAsync(string accountName);
        Task<T> GetAccountAsync(string accountName);
        Task<IEnumerable<T>> GetAccountsAsync(bool forceRefresh = false);
    }
}
