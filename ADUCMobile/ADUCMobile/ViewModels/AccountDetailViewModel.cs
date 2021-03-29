using ADUCMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ADUCMobile.ViewModels
{
    public class AccountDetailViewModel : BaseViewModel
    {
        public Action UpdateSuccesfull;
        public Action<Exception> UpdateFailed;

        public Account Account { get; set; }
        public AccountDetailViewModel(Account account)
        {
            Title = account.AccountName;
            Account = account;
            SubmitCommand = new Command(OnSubmit);
        }

        public ICommand SubmitCommand { protected set; get; }

        public async void OnSubmit()
        {
            try
            {
                await DataStore.UpdateAccountAsync(Account);
                UpdateSuccesfull();
            }
            catch (Exception e)
            {
                UpdateFailed(e);
            }
        }
    }
}
