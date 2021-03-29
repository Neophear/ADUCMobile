using ADUCMobile.Helpers;
using ADUCMobile.Interfaces;
using ADUCMobile.Models;
using ADUCMobile.Models.Exceptions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ADUCMobile.Controllers
{
    public class UserController
    {
        public User LoggedInUser { get; private set; }
        private ICommunicator communicator;
        private static UserController instance;

        private UserController()
        {
            communicator = AppSettings.GetInstance().Communicator;
        }

        public static UserController GetInstance()
        {
            if (instance == null)
                instance = new UserController();

            return instance;
        }

        public void SetToken(string token)
        {
            SecureStorage.SetAsync("token", token);
        }

        public async Task<string> GetToken()
        {
            return await SecureStorage.GetAsync("token");
        }

        public void RemoveToken()
        {
            Task.Run(() => SecureStorage.Remove("token"));
        }

        public async Task<User> AuthenticateToken()
        {
            string token = await GetToken();

            if (string.IsNullOrEmpty(token))
                throw new NotAuthenticatedException();

            string response = await communicator.Get($"{App.ADUCBackendUrl}/api/user/authenticateToken", token);

            return GetUserFromJson(response);
        }

        public async Task<User> Authenticate(string username, string password)
        {
            RemoveToken();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new NotAuthenticatedException();

            var serializedUser = JsonConvert.SerializeObject(new User { Username = username, Password = password });
            string response = await communicator.Post($"{App.ADUCBackendUrl}/api/user/authenticate", serializedUser);

            return GetUserFromJson(response);
        }

        private User GetUserFromJson(string json)
        {
            User user = JsonConvert.DeserializeObject<User>(json);

            SetToken(user.Token);
            LoggedInUser = user;
            return user;
        }
    }
}
