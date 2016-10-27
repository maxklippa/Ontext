using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web.Hosting;
using Ontext.BLL.Providers.Base;
using Ontext.BLL.Services.Contracts;
using Ontext.BLL.ServicesHost.Contracts;
using Ontext.DAL.UnitOfWork.Contracts;
using PushSharp;
using PushSharp.Apple;

namespace Ontext.BLL.Services
{
    public class ApplePushNotificationService : HostService<IApplePushNotificationService>, IApplePushNotificationService
    {
        private readonly byte[] _appleCert;

        public ApplePushNotificationService(IServicesHost servicesHost, IUnitOfWork unitOfWork)
            : base(servicesHost, unitOfWork)
        {
            var appleCertPath = HostingEnvironment.MapPath("~/App_Data/Certificates/Natural20_Ontext_Push_Distribution.p12");
            _appleCert = File.ReadAllBytes(appleCertPath);
        }

        public void SendPushNotifications(IEnumerable<string> userDeviceTokens, string alert, int badge, string customItem)
        {
            var push = new PushBroker();

            push.RegisterAppleService(new ApplePushChannelSettings(true, _appleCert, ""));

            foreach (var deviceToken in userDeviceTokens)
            {
                push.QueueNotification(new AppleNotification()
                    .ForDeviceToken(deviceToken)
                    .WithAlert(alert)
                    .WithBadge(badge)
                    .WithSound("default")
                    .WithCustomItem("MessageId", customItem));
            }

            push.StopAllServices();
        }
    }
}