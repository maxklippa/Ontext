using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ontext.BLL.Providers.Base;
using Ontext.BLL.Providers.Contracts;
using Ontext.BLL.ServicesHost.Contracts;
using Ontext.Core.Objects;
using Ontext.DAL.Models;
using Ontext.DAL.Repositories.Contracts;
using Ontext.DAL.UnitOfWork.Contracts;

namespace Ontext.BLL.Providers
{
    public class ContextsProvider : HostService<IContextsProvider>, IContextsProvider
    {
        #region [ Constructors ]

        public ContextsProvider(IServicesHost servicesHost, IUnitOfWork unitOfWork)
            : base(servicesHost, unitOfWork)
        {
        }

        #endregion // [ Constructors ]

        #region [ Public Methods ]

        public ApiContext GetByName(string name)
        {
            var contextEntity = UnitOfWork.GetRepository<Context>().GetAll().Where(c => !c.IsDeleted).FirstOrDefault(c => c.Name == name);

            return (contextEntity != null) ? Mapper.Map<Context, ApiContext>(contextEntity) : null;
        }

        public List<ApiContext> GetLastModifiedContexts(DateTime lastModifiedDate)
        {
            var contextEntities = UnitOfWork.GetRepository<Context>().GetAll().Where(c => c.LastModifiedDate > lastModifiedDate);

            return Mapper.Map<IQueryable<Context>, List<ApiContext>>(contextEntities);
        }

        public List<ApiContext> GetAll()
        {
            var contextEntities = UnitOfWork.GetRepository<Context>().GetAll().Where(c => !c.IsDeleted);

            return Mapper.Map<IQueryable<Context>, List<ApiContext>>(contextEntities);
        }

        public ApiContext GetById(Guid id)
        {
            var contextEntity = UnitOfWork.GetRepository<Context>().GetById(id);

            return (contextEntity != null && !contextEntity.IsDeleted) ? Mapper.Map<Context, ApiContext>(contextEntity) : null;
        }

        public Guid Save(ApiContext model)
        {
            var contextEntity = UnitOfWork.GetRepository<Context>().GetById(model.Id);

            if (contextEntity == null)
            {
                contextEntity = Mapper.Map<ApiContext, Context>(model);

                contextEntity.CreatedDate = contextEntity.LastModifiedDate = DateTime.UtcNow;

                UnitOfWork.GetRepository<Context>().Insert(contextEntity);
            }
            else
            {
                Mapper.Map(model, contextEntity);

                contextEntity.LastModifiedDate = DateTime.UtcNow;

                UnitOfWork.GetRepository<Context>().Update(contextEntity);
            }

            UnitOfWork.SaveChanges();

            return contextEntity.Id;
        }

        public void Delete(ApiContext model)
        {
            model.IsDeleted = true;
            model.LastModifiedDate = DateTime.UtcNow;
            Save(model);

//            UnitOfWork.GetRepository<Context>().Delete(model.Id);
//            UnitOfWork.SaveChanges();
        }

        public void Delete(Guid modelId)
        {
            var model = GetById(modelId);
            Delete(model);

//            UnitOfWork.GetRepository<Context>().Delete(modelId);
//            UnitOfWork.SaveChanges();
        }

        #endregion // [ Public Methods ]
    }
}