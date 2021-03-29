using ADUCMobile.Controllers;
using ADUCMobile.Models;
using ADUCMobile.Models.Exceptions;
using ADUCMobile.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ADUCMobile.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public IDataStore<Account> DataStore => DependencyService.Get<IDataStore<Account>>();

        private bool isReady;
        public bool IsReady
        {
            get { return isReady; }
            set
            {
                isReady = value;
                PropertyChanged(this, new PropertyChangedEventArgs("IsReady"));
            }
        }

        public Action<Exception> SearchFailed;
        public Action<Account> AccountFound;

        public Account FoundAccount { get; private set; }

        private string accountName;
        public string AccountName
        {
            get { return accountName; }
            set
            {
                accountName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("AccountName"));
            }
        }

        public ICommand SubmitCommand { protected set; get; }

        public SearchViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
            IsReady = true;
        }

        public async void OnSubmit()
        {
            if (!IsReady)
                return;

            IsReady = false;
            
            try
            {
                Account account = await DataStore.GetAccountAsync(accountName);
                AccountFound(account);
            }
            catch (Exception e)
            {
                SearchFailed(e);
            }

            IsReady = true;
        }
    }
}