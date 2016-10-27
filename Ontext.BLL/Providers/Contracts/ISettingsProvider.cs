using System;
using Ontext.Core.Objects;

namespace Ontext.BLL.Providers.Contracts
{
    public interface ISettingsProvider : ICrudService<ApiSettings>
    {
        ApiSettings GetByUserId(Guid userId);
    }
}