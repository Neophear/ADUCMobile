using ADUCMobile.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ADUCMobile.Helpers
{
    class AppSettings
    {
        public ICommunicator Communicator { get; set; }

        private static AppSettings instance;
        private AppSettings()
        {
            //Client = new HttpClient
            //{
            //    BaseAddress = new Uri($"{App.ADUCBackendUrl}/")
            //};

            //Client.DefaultRequestHeaders.Accept.Clear();
            //Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static AppSettings GetInstance()
        {
            if (instance == null)
                instance = new AppSettings();

            return instance;
        }
    }
}
