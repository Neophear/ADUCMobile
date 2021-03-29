using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using ADUCBackendService.Models;
using ADUCMobile.ADUCBackendService.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleImpersonation;

namespace ADUCBackendService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private AppSettings appSettings;

        public AccountController(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        // GET: api/Account
        [HttpGet]
        public IEnumerable<Account> Get()
        {
            return new List<Account>
            {
                new Account{AccountName="Account1", FirstName="FN1", LastName="LN1" },
                new Account{AccountName="Account2", FirstName="FN2", LastName="LN2" },
                new Account{AccountName="Account3", FirstName="FN3", LastName="LN3" }
            };
        }

        // GET: api/Account/5
        [HttpGet("{accountName}", Name = "Get")]
        public async Task<IActionResult> Get(string accountName)
        {
            Account account = await Find(accountName);

            if (account != null)
                return Ok(account);
            else
                return NotFound($"Account {accountName} not found");
        }

        //// POST: api/Account
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{

        //}

        //// PUT: api/Account/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
        
        private bool DoesUserExist(string accountName)
        {
            using (var domainContext = new PrincipalContext(ContextType.Domain, appSettings.DomainName))
                using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, accountName))
                    return foundUser != null;
        }

        [HttpPost]
        public async Task<IActionResult> SaveAccount([FromBody] Account account)
        {
            try
            {
                //Check if account is correctly filled
                if (account == null)
                    return BadRequest(new { Message = "Account can't be null" });
                else if (String.IsNullOrWhiteSpace(account.AccountName))
                    return BadRequest(new { Message = "AccountName can't be nothing" });

                //Get UserPrincipal for account
                await Task.Run(() =>
                {
                    using (var domainContext = new PrincipalContext(ContextType.Domain, appSettings.DomainName))
                        account.up = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, account.AccountName);
                });

                //Check if UserPrincipal exists in Domain
                if (account.up == null)
                    return NotFound(account.AccountName);

                //Set new UserPrincipal properties
                if (!account.Locked)
                    account.up.UnlockAccount();

                if (!String.IsNullOrEmpty(account.Password))
                    account.up.SetPassword(account.Password);
                
                if (account.ChangePasswordAtLogon)
                    account.up.ExpirePasswordNow();

                account.up.AccountExpirationDate = account.Expires;
                account.up.Enabled = account.Enabled;

                //Save UserPrincipal
                account.up.Save();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            finally
            {
                account.up.Dispose();
            }

            return Ok(account);
        }

        private async Task<Account> Find(string accountname)
        {
            Account user = null;
            
            PrincipalContext ouContex = new PrincipalContext(ContextType.Domain, appSettings.DomainName);
            UserPrincipal up = await Task.Run(() => UserPrincipal.FindByIdentity(ouContex, accountname));

            if (up != null)
                user = new Account
                {
                    up = up,
                    AccountName = up.SamAccountName,
                    FirstName = up.GivenName,
                    LastName = up.Surname,
                    Locked = up.IsAccountLockedOut(),
                    Enabled = up.Enabled == null ? true : up.Enabled.Value,
                    Expires = up.AccountExpirationDate
                };

            return user;
        }

        public class AccountNotFoundException : Exception
        {
            public AccountNotFoundException(string accountName)
                : base($"{accountName} does not exist") { }
        }
    }
}