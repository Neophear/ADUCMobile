using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ADUCMobile.Controllers
{
    public class UserController
    {
        private static UserController instance;
        private UserController()
        {

        }

        public static UserController GetInstance()
        {
            if (instance == null)
                instance = new UserController();

            return instance;
        }

        public void SetToken(string token)
        {
            Task.Run(() => SecureStorage.SetAsync("token", token)).Wait();
        }

        public string GetToken()
        {
            Task<string> t = Task.Run(() => SecureStorage.GetAsync("token"));
            t.Wait();

            return t.Result;
        }

        public void RemoveToken()
        {
            Task.Run(() => SecureStorage.Remove("token"));
        }
    }
}
