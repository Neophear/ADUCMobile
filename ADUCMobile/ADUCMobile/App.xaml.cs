using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ADUCMobile.Services;
using ADUCMobile.Views;
using ADUCMobile.Helpers;
using ADUCMobile.Interfaces;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ADUCMobile
{
    public partial class App : Application
    {
        public static string ADUCBackendUrl = "https://aducbs:4870";
        public static string Version = "1.2";

        public App()
        {
            InitializeComponent();
            
            DependencyService.Register<BackendDataStore>();
            AppSettings.GetInstance().Communicator = DependencyService.Get<ICommunicator>();

            MainPage = new NavigationPage(new SearchPage());
            MainPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
