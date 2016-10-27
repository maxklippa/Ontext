using System;
using System.Collections.Generic;
using Ontext.Core.Objects;

namespace Ontext.BLL.Providers.Contracts
{
    public interface IContextsProvider : ICrudService<ApiContext>
    {
        ApiContext GetByName(string name);
        List<ApiContext> GetLastModifiedContexts(DateTime lastModifiedDate);
    }
}