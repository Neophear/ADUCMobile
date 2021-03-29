using ADUCMobile.Controllers;
using ADUCMobile.Models;
using ADUCMobile.Models.Exceptions;
using ADUCMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ADUCMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchPage : ContentPage
	{
		public SearchPage()
		{
            SearchViewModel vm = new SearchViewModel();
            this.BindingContext = vm;
            vm.SearchFailed += SearchFailed;
            vm.AccountFound += (Account account) => Navigation.PushAsync(new AccountDetailPage(account));

            InitializeComponent();
            
            AccountName.Completed += (object sender, EventArgs e) => vm.SubmitCommand.Execute(null);
            lblFooter.Text = $"Lavet af Stiig Gade - Version {App.Version}";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            AccountName.Focus();
        }

        private async void SearchFailed(Exception e)
        {
            await DisplayAlert("Fejl", e.Message, "OK");

            if (e.GetType() == typeof(NotAuthorizedException) || e.GetType() == typeof(NotAuthenticatedException))
                await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            UserController.GetInstance().RemoveToken();
            await Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}