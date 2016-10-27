using Ontext.DAL.Context;
using Ontext.DAL.Context.Contracts;
using Ontext.DAL.Repositories.Contracts;

namespace Ontext.DAL.Repositories.Base
{
    public abstract class BaseRepository : IBaseRepository
    {
        private readonly OntextDbContext _context;

        protected OntextDbContext Context
        {
            get { return _context; }
        }

        protected BaseRepository(IOntextDbContext context)
        {
            _context = context as OntextDbContext;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    } 
}