using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ADUCMobile.Interfaces
{
    public interface ICommunicator
    {
        Task<string> Get(string url, string authToken = null);
        Task<string> Post(string url, string Body, string authToken = null);
    }
}
