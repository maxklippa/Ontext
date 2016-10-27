using System;
using System.Linq;
using Ontext.DAL.Context;
using Ontext.DAL.Repositories.Base;
using Ontext.DAL.Repositories.Contracts;

namespace Ontext.DAL.Repositories
{
    public class ContextsRepository : BaseRepository, IContextsRepository
    {
        #region [ Constructors ]

        public ContextsRepository(OntextDbContext databaseContext)
            :base(databaseContext)
        {

        }

        #endregion // [ Constructors ]

        #region [ Public Methods ]

        public Models.Context GetById(Guid id)
        {
            return Context.Contexts.FirstOrDefault(context => context.Id == id);
        }

        public Models.Context GetByName(string name)
        {
            return Context.Contexts.FirstOrDefault(context => context.Name == name);
        }

        public bool Add(Models.Context context)
        {
            Context.Contexts.Add(context);

            return Context.SaveChanges() > 0;
        }

        public bool Update(Models.Context context)
        {
            var oldContext = GetById(context.Id);

            if (oldContext == null)
                return false;

            oldContext.Name = context.Name;

            return Context.SaveChanges() > 0;
        }

        public bool Delete(Guid id)
        {
            var context = GetById(id);

            if (context == null)
                return false;

            Context.Contexts.Remove(context);

            return Context.SaveChanges() > 0;
        }

        #endregion // [ Public Methods ]
    }
}