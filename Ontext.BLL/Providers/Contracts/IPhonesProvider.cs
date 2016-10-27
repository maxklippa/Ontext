using System.Collections.Generic;
using Ontext.Core.Objects;
using System;

namespace Ontext.BLL.Providers.Contracts
{
    public interface IPhonesProvider : ICrudService<ApiPhone>
    {
        ApiPhone GetByPhoneNumber(string phoneNumber);
        List<ApiPhone> GetUserPhoneNumbers(Guid userId);
    }
}