using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using Ontext.BLL.Providers.Base;
using Ontext.BLL.Providers.Contracts;
using Ontext.BLL.ServicesHost.Contracts;
using Ontext.Core.Objects;
using Ontext.DAL.Identity;
using Ontext.DAL.Models;
using Ontext.DAL.Repositories.Contracts;
using System;
using Ontext.DAL.UnitOfWork.Contracts;

namespace Ontext.BLL.Providers
{
    public class PhonesProvider : HostService<IPhonesProvider>, IPhonesProvider
    {
        #region [ Constructors ]

        public PhonesProvider(IServicesHost servicesHost, IUnitOfWork unitOfWork)
            : base(servicesHost, unitOfWork)
        {
        }

        #endregion // [ Constructors ]

        #region [ Public Methods ]

        public ApiPhone GetByPhoneNumber(string phoneNumber)
        {
            var phoneEntity = UnitOfWork.GetRepository<Phone>().GetAll().FirstOrDefault(p => p.Number == phoneNumber);

            return Mapper.Map<Phone, ApiPhone>(phoneEntity);
        }

        public List<ApiPhone> GetUserPhoneNumbers(Guid userId)
        {
            var phoneEntities = UnitOfWork.GetRepository<Phone>().GetAll().Where(p => p.User != null && p.User.Id == userId);
            
            return Mapper.Map<IQueryable<Phone>, List<ApiPhone>>(phoneEntities);
        }

        public List<ApiPhone> GetAll()
        {
            var phoneEntities = UnitOfWork.GetRepository<Phone>().GetAll().Include(p => p.User).Include(p => p.Contacts);

            return Mapper.Map<IQueryable<Phone>, List<ApiPhone>>(phoneEntities);
        }

        public ApiPhone GetById(Guid id)
        {
            var phoneEntity = UnitOfWork.GetRepository<Phone>().GetById(id);

            return (phoneEntity != null) ? Mapper.Map<Phone, ApiPhone>(phoneEntity) : null;
        }

        public Guid Save(ApiPhone model)
        {
            var phoneEntity = UnitOfWork.GetRepository<Phone>().GetById(model.Id);

            if (phoneEntity == null)
            {
                phoneEntity = Mapper.Map<ApiPhone, Phone>(model);

                UnitOfWork.GetRepository<Phone>().Insert(phoneEntity);
            }
            else
            {
                phoneEntity = Mapper.Map(model, phoneEntity);

                UnitOfWork.GetRepository<Phone>().Update(phoneEntity);
            }

            UnitOfWork.SaveChanges();

            return phoneEntity.Id;
        }

        public void Delete(ApiPhone model)
        {
            UnitOfWork.GetRepository<Phone>().Delete(model.Id);
            UnitOfWork.SaveChanges();
        }

        public void Delete(Guid modelId)
        {
            UnitOfWork.GetRepository<Phone>().Delete(modelId);
            UnitOfWork.SaveChanges();
        }

        #endregion // [ Public Methods ]
    }
}