using System;
using System.Threading.Tasks;
using ADUCMobile.Droid;
using ADUCMobile.Interfaces;
using Android.OS;
using Java.Security;
using Java.Security.Cert;
using Javax.Net.Ssl;
using Xamarin.Forms;
using Square.OkHttp;
using ADUCMobile.Models.Exceptions;

[assembly: Dependency(typeof(Communicator))]
namespace ADUCMobile.Droid
{
    public class Communicator : ICommunicator
    {
        private MediaType JSON = MediaType.Parse("application/json");
        private static OkHttpClient client;
        private static SSLContext trustAllSslContext;

        public Communicator()
        {
            trustAllSslContext = SSLContext.GetInstance("SSL");
            trustAllSslContext.Init(null, new ITrustManager[] { new CustomX509TrustManager() }, new SecureRandom());

            client = new OkHttpClient();
            client.SetConnectTimeout(5, Java.Util.Concurrent.TimeUnit.Seconds);
            client.SetSslSocketFactory(trustAllSslContext.SocketFactory);
            client.SetHostnameVerifier(new CustomHostNameVerifier());
            client.SetProtocols(new Protocol[] { Protocol.Http11 });

            StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().PermitAll().Build();
            StrictMode.SetThreadPolicy(policy);
        }

        public async Task<string> Get(string url, string authToken = null)
        {
            Request request = new Request.Builder().Url(url).Get().Build();
            return await SendRequest(request, authToken);
        }

        public async Task<string> Post(string url, string body, string authToken = null)
        {
            RequestBody requestBody = RequestBody.Create(JSON, body);
            Request request = new Request.Builder().Url(url).Post(requestBody).Build();
            return await SendRequest(request, authToken);
        }

        private async Task<string> SendRequest(Request request, string authToken = null)
        {
            if (authToken != null)
                request = request.NewBuilder().AddHeader("Authorization", $"Bearer {authToken}").Build();

            Response response = null;

            try
            {
                response = await client.NewCall(request).ExecuteAsync();
            }
            catch (Java.Net.UnknownHostException e)
            {
                throw new CommunicationException($"Kunne ikke forbinde til tjenesten:\n{App.ADUCBackendUrl}", e);
            }
            catch (Java.Net.SocketTimeoutException e)
            {
                throw new CommunicationException($"Kunne ikke forbinde til tjenesten:\n{App.ADUCBackendUrl}", e);
            }
            catch (Exception e)
            {
                throw new CommunicationException($"Der var et problem med forbindelsen til tjenesten:\n{e.Message}", e);
            }

            switch (response?.Code())
            {
                case 200:
                    return response.Body().String();
                case 400:
                    throw new NotAuthenticatedException();
                case 401:
                    throw new NotAuthorizedException();
                case 404:
                    throw new NotFoundException();
                case 500:
                    throw new CommunicationException("Fejl på serveren!");
                default:
                    throw new CommunicationException("Noget gik galt med forbindelsen til serveren!");
            }
        }

        private class CustomHostNameVerifier : Java.Lang.Object, IHostnameVerifier
        {
            public bool Verify(string hostname, ISSLSession session) { return true;}
        }

        private class CustomX509TrustManager : Java.Lang.Object, IX509TrustManager
        {
            public void CheckClientTrusted(X509Certificate[] chain, string authType) { }
            public void CheckServerTrusted(X509Certificate[] chain, string authType) { }
            X509Certificate[] IX509TrustManager.GetAcceptedIssuers() { return new X509Certificate[0]; }
        }
    }
}