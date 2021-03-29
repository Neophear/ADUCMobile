using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ADUCBackendService.Models;
using ADUCBackendService.Models.Exceptions;
using ADUCMobile.ADUCBackendService.Helpers;
using ADUCMobile.ADUCBackendService.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ADUCMobile.ADUCBackendService.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> AuthenticateToken(string username);
        Task<User> GetUser(string username);
    }
    public class UserService : IUserService
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);

        private readonly AppSettings appSettings;
        //private List<User> users = new List<User>
        //{
        //    new User { FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        //};

        public UserService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
                throw new NotAuthenticatedException();

            return await AuthenticateMethod(username, password);
        }

        public async Task<User> AuthenticateToken(string username)
        {
            return await AuthenticateMethod(username);
        }

        private async Task<User> AuthenticateMethod(string username, string password = null)
        {
            return await Task.Run(() =>
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "TRR-INET", "DC = TRR-INET,DC = LOCAL"))
                {
                    using (UserPrincipal up = UserPrincipal.FindByIdentity(pc, username))
                    {
                        if (up == null || (password != null && !pc.ValidateCredentials(username, password, ContextOptions.Negotiate)))
                            throw new NotAuthenticatedException();

                        if (!up.IsMemberOf(pc, IdentityType.Name, "IT-Admins"))
                            throw new NotAuthorizedException();

                        return new User
                        {
                            Username = username,
                            FirstName = up.GivenName,
                            LastName = up.Surname,
                            Token = CreateToken(username)
                        };
                    }
                }
            });
        }

        public async Task<User> GetUser(string username)
        {
            return await Task.Run(() =>
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "TRR-INET", "DC = TRR-INET,DC = LOCAL"))
                {
                    UserPrincipal up = UserPrincipal.FindByIdentity(pc, username);
                    if (up != null)
                        return new User
                        {
                            Username = username,
                            FirstName = up.GivenName,
                            LastName = up.Surname
                        };
                    else
                        return null;
                }
            });
        }

        private string CreateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}