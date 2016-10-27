using System;
using Ontext.DAL.Models;

namespace Ontext.DAL.Repositories.Contracts
{
    public interface IContextsRepository : IBaseRepository
    {
        Models.Context GetById(Guid id);
        Models.Context GetByName(string name);

        Boolean Add(Models.Context context);
        Boolean Update(Models.Context context);
        Boolean Delete(Guid id);
    }
}