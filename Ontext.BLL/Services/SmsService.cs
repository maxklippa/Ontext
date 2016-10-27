using System.Collections.Generic;
using Ontext.BLL.Providers.Base;
using Ontext.BLL.Services.Contracts;
using Ontext.BLL.ServicesHost.Contracts;
using Ontext.DAL.UnitOfWork.Contracts;
using Ontext.Server.Core.Settings;
using Twilio;

namespace Ontext.BLL.Services
{
    public class SmsService : HostService<ISmsService>, ISmsService
    {
        public SmsService(IServicesHost servicesHost, IUnitOfWork unitOfWork) : base(servicesHost, unitOfWork)
        {
        }

        public void SendSmsMessage(string destination, string body)
        {
            var twilio = new TwilioRestClient(OntextSettings.TwilioSid, OntextSettings.TwilioToken);
            var res = twilio.SendMessage(OntextSettings.TwilioPhoneNumber, destination, body);
        }

        public void SendMessage(string destination, string body, string[] mediaUrls)
        {
            var twilio = new TwilioRestClient(OntextSettings.TwilioSid, OntextSettings.TwilioToken);
            var res = twilio.SendMessage(OntextSettings.TwilioPhoneNumber, destination, body, mediaUrls);
        }
    }
}
