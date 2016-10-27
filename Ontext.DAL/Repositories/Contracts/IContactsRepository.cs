using Ontext.DAL.Models;
using System;
using System.Collections.Generic;

namespace Ontext.DAL.Repositories.Contracts
{
    public interface IContactsRepository : IBaseRepository
    {
        Contact GetById(Guid id);
        IEnumerable<Contact> GetContactsByUserId(Guid userId);

        Boolean Add(Contact contact);
        Boolean Update(Contact contact);
        Boolean Delete(Guid id);
    }
}