using System;
using System.Collections.Generic;
using System.Text;

namespace ADUCMobile.Models
{
    public class Account
    {
        public string AccountName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Locked { get; set; }
        public bool Enabled { get; set; }
        public DateTime? Expires { get; set; }
    }
}
