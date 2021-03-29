using System;
using System.Collections.Generic;
using System.Text;

namespace ADUCMobile.Models.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException()
            : base("Ingen adgang!")
        { }
    }
}
