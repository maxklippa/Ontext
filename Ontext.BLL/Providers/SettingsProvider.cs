using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ontext.BLL.Providers.Base;
using Ontext.BLL.Providers.Contracts;
using Ontext.BLL.ServicesHost.Contracts;
using Ontext.Core.Objects;
using Ontext.DAL.Identity;
using Ontext.DAL.Models;
using Ontext.DAL.UnitOfWork.Contracts;

namespace Ontext.BLL.Providers
{
    public class SettingsProvider : HostService<ISettingsProvider>, ISettingsProvider
    {
        public SettingsProvider(IServicesHost servicesHost, IUnitOfWork unitOfWork) : base(servicesHost, unitOfWork)
        {
        }

        public List<ApiSettings> GetAll()
        {
            var settingsEntities = UnitOfWork.GetRepository<Settings>().GetAll();
            return Mapper.Map<IQueryable<Settings>, List<ApiSettings>>(settingsEntities);
        }

        public ApiSettings GetById(Guid id)
        {
            var settingsEntity = UnitOfWork.GetRepository<Settings>().GetById(id);
            return (settingsEntity != null) ? Mapper.Map<Settings, ApiSettings>(settingsEntity) : null;
        }

        public Guid Save(ApiSettings model)
        {
            var settingsEntity = UnitOfWork.GetRepository<Settings>().GetById(model.Id);

            if (settingsEntity == null)
            {
                settingsEntity = Mapper.Map<ApiSettings, Settings>(model);

                BuildSettings(model, settingsEntity);
                
                UnitOfWork.GetRepository<Settings>().Insert(settingsEntity);
            }
            else
            {
                settingsEntity = Mapper.Map(model, settingsEntity);

                BuildSettings(model, settingsEntity);

                UnitOfWork.GetRepository<Settings>().Update(settingsEntity);
            }

            UnitOfWork.SaveChanges();

            return settingsEntity.Id;
        }
        
        public void Delete(ApiSettings model)
        {
            UnitOfWork.GetRepository<Settings>().Delete(model.Id);
            UnitOfWork.SaveChanges();
        }

        public void Delete(Guid modelId)
        {
            UnitOfWork.GetRepository<Settings>().Delete(modelId);
            UnitOfWork.SaveChanges();
        }

        public ApiSettings GetByUserId(Guid userId)
        {
            var settingsEntity = UnitOfWork.GetRepository<Settings>().GetAll().FirstOrDefault(s => s.Id == userId);
            return (settingsEntity != null) ? Mapper.Map<Settings, ApiSettings>(settingsEntity) : null;
        }

        private void BuildSettings(ApiSettings settings, Settings settingsEntity)
        {
            var user = UnitOfWork.GetRepository<OntextUser>().GetById(settings.Id);

            if (user == null)
            {
                throw new ArgumentException("Related entities do not exist.");
            }

            settingsEntity.User = user;
        }
    }
}