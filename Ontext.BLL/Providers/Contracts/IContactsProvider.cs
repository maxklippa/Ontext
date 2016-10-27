using Ontext.Core.Objects;
using System;
using System.Collections.Generic;

namespace Ontext.BLL.Providers.Contracts
{
    public interface IContactsProvider : ICrudService<ApiContact>
    {
        List<ApiContact> GetContacts(Guid userId);
        List<ApiContact> GetActiveContacts(Guid userId);
        List<ApiContact> GetBlockedContacts(Guid userId);
        List<ApiPhone> GetContactsWithApp(string[] phones);
        ApiContact GetByPhoneNumber(Guid userId, string phoneNumber);
        List<ApiContact> GetLastModifiedContacts(Guid userId, DateTime lastModifiedDate);
    }
}