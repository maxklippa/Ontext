using System;
using System.Collections.Generic;
using Ontext.Core.Objects;
using Ontext.DAL.Models;

namespace Ontext.BLL.Providers.Contracts
{
    public interface IMessagesProvider : ICrudService<ApiMessage>
    {
        List<ApiMessage> GetUserMessages(Guid userId, bool unreadOnly = true);
        void DeleteAll(Guid userId);
        int UnreadUserMessagesCount(Guid userId);
    }
}