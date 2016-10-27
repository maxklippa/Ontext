using System;
using System.Collections.Generic;
using Ontext.DAL.Models;

namespace Ontext.DAL.Repositories.Contracts
{
    public interface IMessagesRepository : IBaseRepository
    {
        Message GetById(Guid id);
        IEnumerable<Message> GetMessagesByUserId(Guid userId);

        Boolean Add(Message message);
        Boolean Update(Message message);
        Boolean Delete(Guid id);
    }
}