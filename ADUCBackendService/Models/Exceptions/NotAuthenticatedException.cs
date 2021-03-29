using System;
using System.Collections.Generic;
using System.Text;

namespace ADUCBackendService.Models.Exceptions
{
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException()
            : base("Not authenticated!")
        { }
    }
}
