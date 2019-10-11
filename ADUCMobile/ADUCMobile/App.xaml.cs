using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ADUCMobile.Services;
using ADUCMobile.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ADUCMobile
{
    public partial class App : Application
    {
        public static string ADUCBackendUrl = "http://192.168.1.171:45455";
        public static bool UseMockDataStore = false;

        public App()
        {
            InitializeComponent();

            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<BackendDataStore>();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
