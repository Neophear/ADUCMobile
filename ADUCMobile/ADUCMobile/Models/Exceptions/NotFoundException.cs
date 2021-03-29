using System;
using System.Collections.Generic;
using System.Text;

namespace ADUCMobile.Models.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : this("Ikke fundet!")
        { }
        
        public NotFoundException(string message)
            : base(message)
        { }
    }
}
