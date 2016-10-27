using Ontext.BLL.Providers.Contracts;

namespace Ontext.BLL.Services.Contracts
{
    public interface ISmsService : IService
    {
        void SendSmsMessage(string destination, string body);
        void SendMessage(string destination, string body, string[] mediaUrls);
    }
}