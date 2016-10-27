using System;
using Ontext.DAL.Models;

namespace Ontext.DAL.Repositories.Contracts
{
    public interface IPhonesRepository : IBaseRepository
    {
        Phone GetById(Guid id);
        Phone GetByPhoneNumber(string phoneNumber);

        Boolean Add(Phone phone);
        Boolean Update(Phone phone);
        Boolean Delete(Guid id);
    }
}