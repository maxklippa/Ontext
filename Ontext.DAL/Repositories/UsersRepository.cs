using System.Data.Entity;
using System.Linq;
using Ontext.DAL.Context;
using Ontext.DAL.Identity;
using Ontext.DAL.Models;
using Ontext.DAL.Repositories.Base;
using Ontext.DAL.Repositories.Contracts;
using System;

namespace Ontext.DAL.Repositories
{
    public class UsersRepository : BaseRepository, IUsersRepository
    {
        #region [ Constructors ]

        public UsersRepository(OntextDbContext databaseContext)
            : base(databaseContext)
        {

        }

        #endregion // [ Constructors ]

        #region [ Public Methods ]

        public OntextUser GetById(Guid id)
        {
            return Context.Users.FirstOrDefault(user => user.Id == id);
        }

        public OntextUser GetByPhoneNumber(string phoneNumber)
        {
            return Context.Users.FirstOrDefault(a => a.Phones.Any(p => p.Number == phoneNumber));
        }

        public OntextUser GetByEmailAddress(string emailAddress)
        {
            return Context.Users.FirstOrDefault(user => user.Email == emailAddress);
        }

        public bool Add(OntextUser user)
        {
            if (!user.Phones.Any())
            {
                throw new Exception("Can not create an user without a phone.");
            }

            if (user.Phones.Any(p => p.User != null))
            {
                throw new Exception("Can not create an user. One of the phones belongs to another user.");
            }

            Context.Users.Add(user);

            return Context.SaveChanges() > 0;
        }

        public bool Update(OntextUser user)
        {
            var oldUser = GetById(user.Id);

            if (oldUser == null)
                return false;

            oldUser.Email = user.Email;

            return Context.SaveChanges() > 0;
        }

        #endregion // [ Public Methods ]
    }
}