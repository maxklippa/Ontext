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
using System.Collections.Generic;
using Ontext.DAL.UnitOfWork.Contracts;

namespace Ontext.BLL.Providers
{
    public class ContactsProvider : HostService<IContactsProvider>, IContactsProvider
    {
        #region [ Constructors ]

        public ContactsProvider(IServicesHost servicesHost, IUnitOfWork unitOfWork)
            : base(servicesHost, unitOfWork)
        {
        }

        #endregion // [ Constructors ]

        #region [ Public Methods ]

        public List<ApiContact> GetContacts(Guid userId)
        {
            var contactEntities =
                UnitOfWork.GetRepository<Contact>()
                    .GetAll()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.Phone)
                    .Where(c => c.User.Id == userId);

            return Mapper.Map<IQueryable<Contact>, List<ApiContact>>(contactEntities);
        }

        public List<ApiContact> GetActiveContacts(Guid userId)
        {
            var contactEntities =
                UnitOfWork.GetRepository<Contact>()
                    .GetAll()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.Phone)
                    .Where(c => c.User.Id == userId && !c.Blocked);
            
            return Mapper.Map<IQueryable<Contact>, List<ApiContact>>(contactEntities);
        }

        public List<ApiContact> GetBlockedContacts(Guid userId)
        {
            var contactEntities =
                UnitOfWork.GetRepository<Contact>()
                    .GetAll()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.Phone)
                    .Where(c => c.User.Id == userId && c.Blocked);

            return Mapper.Map<IQueryable<Contact>, List<ApiContact>>(contactEntities);
        }
        
        public List<ApiPhone> GetContactsWithApp(string[] phones)
        {
            var phoneEntities =
                UnitOfWork.GetRepository<Phone>()
                    .GetAll()
                    .Where(p => p.UserId.HasValue && phones.Contains(p.Number));

            return Mapper.Map<IQueryable<Phone>, List<ApiPhone>>(phoneEntities);
        }

        public ApiContact GetByPhoneNumber(Guid userId, string phoneNumber)
        {
            var contactEntity =
                UnitOfWork.GetRepository<Contact>()
                    .GetAll()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.Phone)
                    .FirstOrDefault(c => c.Phone.Number == phoneNumber && c.UserId == userId);

            return Mapper.Map<Contact, ApiContact>(contactEntity);
        }

        public List<ApiContact> GetLastModifiedContacts(Guid userId, DateTime lastModifiedDate)
        {
            var contactEntities =
                UnitOfWork.GetRepository<Contact>()
                    .GetAll()
                    .Include(c => c.Phone)
                    .Where(c => c.User.Id == userId && c.LastModifiedDate > lastModifiedDate);

            return Mapper.Map<IQueryable<Contact>, List<ApiContact>>(contactEntities);
        }

        public List<ApiContact> GetAll()
        {
            var store =
                UnitOfWork.GetRepository<Contact>()
                    .GetAll()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.User)
                    .Include(c => c.Phone)
                    .Include(c => c.Context);

            return Mapper.Map<IQueryable<Contact>, List<ApiContact>>(store);
        }

        public ApiContact GetById(Guid id)
        {
            var store =
                UnitOfWork.GetRepository<Contact>()
                    .GetAll()
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.Phone)
                    .FirstOrDefault(c => c.Id == id);

            return (store != null) ? Mapper.Map<Contact, ApiContact>(store) : null;
        }

        public Guid Save(ApiContact model)
        {
            var contactEntity = UnitOfWork.GetRepository<Contact>().GetById(model.Id);

            SetContactPhone(model);

            if (contactEntity == null)
            {
                contactEntity = Mapper.Map<ApiContact, Contact>(model);

                contactEntity.CreatedDate = contactEntity.LastModifiedDate = DateTime.UtcNow;

                UnitOfWork.GetRepository<Contact>().Insert(contactEntity);
            }
            else
            {
                contactEntity = Mapper.Map(model, contactEntity);

                contactEntity.LastModifiedDate = DateTime.UtcNow;

                UnitOfWork.GetRepository<Contact>().Update(contactEntity);
            }

            UnitOfWork.SaveChanges();

            return contactEntity.Id;
        }

        public void Delete(ApiContact model)
        {
            model.IsDeleted = true;
            model.LastModifiedDate = DateTime.UtcNow;
            Save(model);

//            UnitOfWork.GetRepository<Contact>().Delete(model.Id);
//            UnitOfWork.SaveChanges();
        }

        public void Delete(Guid modelId)
        {
            var model = GetById(modelId);
            Delete(model);

//            UnitOfWork.GetRepository<Contact>().Delete(modelId);
//            UnitOfWork.SaveChanges();
        }

        #endregion // [ Public Methods ]

        #region [ Private Methods ]
        
        private void SetContactPhone(ApiContact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.PhoneNumber)) return;

            var phone = UnitOfWork.GetRepository<Phone>().GetAll().FirstOrDefault(p => p.Number == contact.PhoneNumber);

            if (phone == null)
            {
                phone = new Phone
                {
                    Number = contact.PhoneNumber
                };
                UnitOfWork.GetRepository<Phone>().Insert(phone);

                UnitOfWork.SaveChanges();
            }

            contact.PhoneId = phone.Id;
        }

        #endregion // [ Private Methods ]
    }
}