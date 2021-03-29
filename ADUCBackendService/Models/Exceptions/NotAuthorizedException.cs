using System;
using System.Collections.Generic;
using System.Text;

namespace ADUCBackendService.Models.Exceptions
{
    class NotAuthorizedException : Exception
    {
        public NotAuthorizedException()
            : base("Not authorized!")
        { }
    }
}
