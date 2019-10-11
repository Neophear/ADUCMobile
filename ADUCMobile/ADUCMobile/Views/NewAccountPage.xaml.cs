using ADUCMobile.Models;
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
	public partial class NewAccountPage : ContentPage
	{
        public Account Account { get; set; }
		public NewAccountPage()
		{
			InitializeComponent ();

            Account = new Account
            {
                AccountName = "Accountname",
                FirstName = "This is first name",
                LastName = "This is last name"
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddAccount", Account);
            await Navigation.PopModalAsync();
        }
        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}