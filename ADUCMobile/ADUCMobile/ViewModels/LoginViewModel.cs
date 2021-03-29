using ADUCMobile.Controllers;
using ADUCMobile.Helpers;
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
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Action<Exception> LoginFailed;
        public Action LoginSuccesful;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private bool isReady;
        public bool IsReady
        {
            get { return isReady; }
            set
            {
                isReady = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsReady"));
            }
        }

        private string username = "";
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Username"));
            }
        }

        private string password = "";
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }

        public ICommand SubmitCommand { protected set; get; }
        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
            IsReady = true;
        }

        public async Task CheckToken()
        {
            IsReady = false;

            try
            {
                if (await DependencyService.Get<IDataStore<Account>>().CheckConnection()
                    && await UserController.GetInstance().AuthenticateToken() != null)
                        LoginSuccesful();
            }
            catch (Exception e) when (e is NotAuthenticatedException || e is NotAuthorizedException)
            { }
            catch (Exception e)
            {
                LoginFailed(e);
            }
            finally
            {
                IsReady = true;
            }
        }

        public async void OnSubmit()
        {
            if (!IsReady)
                return;

            IsReady = false;
            
            try
            {
                User user = await UserController.GetInstance().Authenticate(username, password);
                LoginSuccesful();
            }
            catch (Exception e) when (e is NotAuthorizedException || e is NotAuthenticatedException)
            {
                Password = "";
                LoginFailed(e);
            }
            catch (Exception e)
            {
                LoginFailed(e);
            }
            finally
            {
                IsReady = true;
            }
        }
    }
}