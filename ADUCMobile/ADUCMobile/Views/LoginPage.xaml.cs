using ADUCMobile.Controllers;
using ADUCMobile.Models;
using ADUCMobile.Models.Exceptions;
using ADUCMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace ADUCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        LoginViewModel vm;

        public LoginPage()
		{
            vm = new LoginViewModel();
            this.BindingContext = vm;

            vm.LoginFailed += LoginFailed;
            vm.LoginSuccesful += () => Navigation.PopModalAsync();

            InitializeComponent();

            Username.Completed += (object sender, EventArgs e) => Password.Focus();
            Password.Completed += (object sender, EventArgs e) => vm.SubmitCommand.Execute(null);
        }

        protected override async void OnAppearing()
        {
            await vm.CheckToken();
            base.OnAppearing();
        }

        private async void LoginFailed(Exception e)
        {
            if (e.GetType() == typeof(NotAuthenticatedException) || e.GetType() == typeof(NotAuthorizedException))
                await DisplayAlert("Fejl", $"{e.Message}", "OK");
            else if (await DisplayAlert("Fejl", $"{e.Message}\nKopier fejlen til clipboard?", "Ja", "Nej"))
                await Clipboard.SetTextAsync(e.ToString());
        }
    }
}