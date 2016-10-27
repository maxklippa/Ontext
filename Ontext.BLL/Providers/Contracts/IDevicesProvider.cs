using System;
using System.Collections.Generic;
using Ontext.Core.Objects;

namespace Ontext.BLL.Providers.Contracts
{
    public interface IDevicesProvider : ICrudService<ApiDevice>
    {
        ApiDevice GetByToken(string token);
        List<ApiDevice> GetUserDevices(Guid userId);
    }
}