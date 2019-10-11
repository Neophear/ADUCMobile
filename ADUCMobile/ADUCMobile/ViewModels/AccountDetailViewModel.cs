using ADUCMobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADUCMobile.ViewModels
{
    public class AccountDetailViewModel : BaseViewModel
    {
        public Account Account { get; set; }
        public AccountDetailViewModel(Account account = null)
        {
            Title = account?.AccountName;
            Account = account;
        }
    }
}
