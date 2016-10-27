using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Ontext.Server.Core.Settings;
using Twilio;

namespace Ontext.BLL.Identity.Services
{
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var accountSid = OntextSettings.TwilioSid;
            var authToken = OntextSettings.TwilioToken;
            var twilioPhoneNumber = OntextSettings.TwilioPhoneNumber;

            var twilio = new TwilioRestClient(accountSid, authToken);
            var res = twilio.SendMessage(twilioPhoneNumber, message.Destination, message.Body);

            // Plug in your sms service here to send a text message.
            return Task.FromResult(0);
        }
    }
}