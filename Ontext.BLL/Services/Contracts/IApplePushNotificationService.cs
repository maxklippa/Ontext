using System;
using System.Collections.Generic;
using Ontext.BLL.Providers.Contracts;

namespace Ontext.BLL.Services.Contracts
{
    public interface IApplePushNotificationService : IService
    {
        void SendPushNotifications(IEnumerable<string> userDeviceTokens, string alert, int badge, string customItem);
    }
}