using System.Threading.Tasks;

namespace coreAppSMS.Api
{
    public interface IWorker
    {

        bool IsInitialized { get; set; }

        void Init();

        Task<Response> SendSmsAsync(string to, string msg);
    }
}