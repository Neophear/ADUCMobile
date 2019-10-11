using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace ADUCBackendService.Models
{
    public class Account
    {
        [JsonIgnore]
        public UserPrincipal up { get; set; }
        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Locked { get; set; }
        public bool Enabled { get; set; }
        public DateTime? Expires { get; set; }
    }
}
