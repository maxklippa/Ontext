using System;
using Ontext.DAL.Identity;
using Ontext.DAL.Models;

namespace Ontext.DAL.Repositories.Contracts
{
    public interface IUsersRepository : IBaseRepository
    {
        OntextUser GetById(Guid id);
        OntextUser GetByPhoneNumber(string phoneNumber);
        OntextUser GetByEmailAddress(string emailAddress);

        Boolean Add(OntextUser user);
        Boolean Update(OntextUser user);
    }
}