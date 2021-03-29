using ADUCMobile.Helpers;
using ADUCMobile.Models;
using ADUCMobile.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ADUCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AccountDetailPage : ContentPage
	{
        RandomPassword randomPassword = new RandomPassword(1, -1, 1, -1, 0, 0, 1, -1);

		public AccountDetailPage(Account account)
		{
            AccountDetailViewModel vm = new AccountDetailViewModel(account);
            BindingContext = vm;
            vm.UpdateSuccesfull += () => DisplayAlert("Succes", "Brugeren blev gemt", "OK");
            vm.UpdateFailed += (Exception e) => DisplayAlert("Fejl", e.Message, "OK");
            InitializeComponent();
        }

        private void GeneratePassword(object sender, EventArgs e)
        {
            Password.Text = randomPassword.GeneratePassword(9);
            ChangePasswordAtLogon.IsToggled = false;
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.NewTextValue))
                ChangePasswordAtLogon.IsToggled = true;
        }
    }
}