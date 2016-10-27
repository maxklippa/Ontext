using Ontext.DAL.Models.Base;
using Ontext.DAL.Repositories.Contracts;
using Ontext.DAL.UnitOfWork.Contracts;

namespace Ontext.DAL.Repositories.Base
{

    public class CustomRepository<TRepository, TEntity> : EntityRepository<TEntity>, ICustomRepository<TEntity>
        where TRepository : ICustomRepository
        where TEntity : class
    {
        /// <summary>
        /// Creates custom repository
        /// </summary>
        /// <param name="unitOfWork"></param>
        public CustomRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork.Context)
        {
            unitOfWork.RegisterCustomRepository((TRepository)(this as ICustomRepository));
        }
    }
}
