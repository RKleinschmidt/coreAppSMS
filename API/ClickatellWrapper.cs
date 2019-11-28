using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System;
namespace coreAppSMS.Api
{
    public class ClickatellWrapperClient : IWorker
    {

        private readonly string m_auth;

        public bool CanCall { get { return false; } }

        public bool IsInitialized { get; set; }

        public ClickatellWrapperClient(string api)
        {
            m_auth = api;
        }

        public void Init()
        {
            IsInitialized = true;
            ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
        }

        public async Task<Response> SendSmsAsync(string to, string msg)
        {
            var req = WebRequest.CreateHttp("https://platform.clickatell.com/messages");
            req.ContentType = "application/json";
            req.Accept = "application/json";
            req.Method = "POST";

            req.PreAuthenticate = true;
            req.Headers.Add(HttpRequestHeader.Authorization, m_auth);

            using (var writer = new StreamWriter(await req.GetRequestStreamAsync()))
            {
                await writer.WriteAsync(new Request(to, msg).ToJson());
                await writer.FlushAsync();
            }

            using (var reader = new StreamReader((await req.GetResponseAsync()).GetResponseStream()))
            {
                var json = await reader.ReadToEndAsync();
                Console.WriteLine(json);
                return JsonConvert.DeserializeObject<Response>(json);
            }
        }
    }
}