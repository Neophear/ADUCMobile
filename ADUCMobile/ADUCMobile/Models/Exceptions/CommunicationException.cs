using System;
using System.Collections.Generic;
using System.Text;

namespace ADUCMobile.Models.Exceptions
{
    public class CommunicationException : Exception
    {
        public CommunicationException(string message)
            : base(message)
        { }

        public CommunicationException(Exception ex)
            : base("Der var et problem med forbindelsen!", ex)
        { }

        public CommunicationException(string message, Exception ex)
            : base(message, ex)
        { }
    }
}