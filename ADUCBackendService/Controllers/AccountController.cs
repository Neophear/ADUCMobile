using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using ADUCBackendService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADUCBackendService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
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

        // POST: api/Account
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

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
            using (var domainContext = new PrincipalContext(ContextType.Domain, "TRR-INET.local"))
                using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, accountName))
                    return foundUser != null;
        }

        private async Task<Account> Find(string accountname)
        {
            Account user = null;
            
            PrincipalContext ouContex = new PrincipalContext(ContextType.Domain, "TRR-INET.local", "DC = TRR-INET,DC = LOCAL");
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
    }
}
