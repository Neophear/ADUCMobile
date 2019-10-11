using ADUCMobile.Models;
using ADUCMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ADUCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountDetailPage : ContentPage
	{
        AccountDetailViewModel viewModel;
		public AccountDetailPage(AccountDetailViewModel viewModel)
		{
			InitializeComponent ();

            BindingContext = this.viewModel = viewModel;
		}

        public AccountDetailPage()
        {
            InitializeComponent();

            var account = new Account
            {
                AccountName = "Konto1",
                FirstName = "Firstname1",
                LastName = "Lastname1"
            };

            viewModel = new AccountDetailViewModel(account);
            BindingContext = viewModel;
        }
	}
}