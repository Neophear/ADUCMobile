using System;
using System.Collections.Generic;
using System.Text;

namespace ADUCMobile.Models.Exceptions
{
    public class NotAuthenticatedException : Exception
    {
        public NotAuthenticatedException()
            : base("Fejl i brugernavn eller adgangskode!")
        { }
    }
}
