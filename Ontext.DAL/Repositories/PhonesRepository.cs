using System;
using System.Linq;
using Ontext.DAL.Context;
using Ontext.DAL.Models;
using Ontext.DAL.Repositories.Base;
using Ontext.DAL.Repositories.Contracts;

namespace Ontext.DAL.Repositories
{
    public class PhonesRepository : BaseRepository, IPhonesRepository
    {
        #region [ Constructors ]

        public PhonesRepository(OntextDbContext databaseContext)
            :base(databaseContext)
        {

        }

        #endregion // [ Constructors ]

        #region [ Public Methods ]

        public Phone GetById(Guid id)
        {
            return Context.Phones.FirstOrDefault(phone => phone.Id == id);
        }

        public Phone GetByPhoneNumber(string phoneNumber)
        {
            return Context.Phones.FirstOrDefault(phone => phone.Number == phoneNumber);
        }

        public bool Add(Phone phone)
        {
            Context.Phones.Add(phone);

            return Context.SaveChanges() > 0;
        }

        public bool Update(Phone phone)
        {
            var oldPhone = GetById(phone.Id);

            if (oldPhone == null)
                return false;

            if (oldPhone.User != null && phone.User != null && oldPhone.User.Id != phone.User.Id)
            {
                throw new Exception("Unable to set the phone account, the phone is already used by another account.");
            }

            if (oldPhone.User != null && phone.User == null && oldPhone.User.Phones.Count == 1)
            {
                throw new Exception("Can not remove phone account. This phone is the LAST account phone.");
            }

            oldPhone.User = phone.User;
            oldPhone.Contact = phone.Contact;
            oldPhone.Number = phone.Number;
            oldPhone.Priority = phone.Priority;

            return Context.SaveChanges() > 0;
        }

        public bool Delete(Guid id)
        {
            var phone = GetById(id);
            
            if (phone == null)
                return false;

            if (phone.User.Phones.Count == 1 && string.IsNullOrWhiteSpace(phone.User.Email))
            {
                throw new Exception("Can not remove the LAST account phone.");
            }

            Context.Phones.Remove(phone);

            return Context.SaveChanges() > 0;
        }

        #endregion // [ Public Methods ]
    }
}