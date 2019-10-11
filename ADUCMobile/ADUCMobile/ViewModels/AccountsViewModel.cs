using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using ADUCMobile.Models;
using ADUCMobile.Views;

namespace ADUCMobile.ViewModels
{
    public class AccountsViewModel : BaseViewModel
    {
        public ObservableCollection<Account> Accounts { get; set; }
        public Command LoadAccountsCommand { get; set; }

        public AccountsViewModel()
        {
            Title = "Browse";
            Accounts = new ObservableCollection<Account>();
            LoadAccountsCommand = new Command(async () => await ExecuteLoadAccountsCommand());

            MessagingCenter.Subscribe<NewAccountPage, Account>(this, "AddAccount", async (obj, account) =>
            {
                var newAccount = account as Account;
                Accounts.Add(newAccount);
                await DataStore.AddAccountAsync(newAccount);
            });
        }

        async Task ExecuteLoadAccountsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Accounts.Clear();
                var accounts = await DataStore.GetAccountsAsync(true);
                foreach (var account in accounts)
                {
                    Accounts.Add(account);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}