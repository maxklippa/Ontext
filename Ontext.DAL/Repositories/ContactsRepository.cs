using Ontext.DAL.Context;
using Ontext.DAL.Models;
using Ontext.DAL.Repositories.Base;
using Ontext.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ontext.DAL.Repositories
{
    public class ContactsRepository : BaseRepository, IContactsRepository
    {
        #region [ Constructors ]

        public ContactsRepository(OntextDbContext databaseContext)
            : base(databaseContext)
        {

        }

        #endregion // [ Constructors ]

        #region [ Public Methods ]

        public Contact GetById(Guid id)
        {
            return Context.Contacts.FirstOrDefault(contact => contact.Id == id);
        }

        public IEnumerable<Contact> GetContactsByUserId(Guid userId)
        {
            var contacts = Context.Contacts.Where(c => c.User.Id == userId);

            return contacts;
        }

        public bool Add(Contact contact)
        {
            Context.Contacts.Add(contact);

            return Context.SaveChanges() > 0;
        }

        public bool Update(Contact contact)
        {
            var oldContact = GetById(contact.Id);

            if (oldContact == null)
                return false;

            oldContact.User = contact.User;
            oldContact.Blocked = contact.Blocked;
            oldContact.Context = contact.Context;
            oldContact.Name = contact.Name;
            oldContact.Phone = contact.Phone;

            return Context.SaveChanges() > 0;
        }

        public bool Delete(Guid id)
        {
            var contact = GetById(id);

            if (contact == null)
                return false;

            Context.Contacts.Remove(contact);

            return Context.SaveChanges() > 0;
        }
        
        #endregion // [ Public Methods ]
    }
}