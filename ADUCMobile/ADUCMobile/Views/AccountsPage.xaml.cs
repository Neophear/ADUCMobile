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
    public partial class AccountsPage : ContentPage
    {
        AccountsViewModel viewModel;

        public AccountsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new AccountsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var account = args.SelectedItem as Account;
            if (account == null)
                return;

            await Navigation.PushAsync(new AccountDetailPage(new AccountDetailViewModel(account)));

            // Manually deselect item.
            AccountsListView.SelectedItem = null;
        }

        async void AddAccount_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewAccountPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Accounts.Count == 0)
                viewModel.LoadAccountsCommand.Execute(null);
        }
    }
}