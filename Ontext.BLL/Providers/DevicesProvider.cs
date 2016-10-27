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
    public class DevicesProvider : HostService<IDevicesProvider>, IDevicesProvider
    {
        public DevicesProvider(IServicesHost servicesHost, IUnitOfWork unitOfWork) 
            : base(servicesHost, unitOfWork)
        {
        }

        public List<ApiDevice> GetAll()
        {
            var deviceEntities = UnitOfWork.GetRepository<Device>().GetAll();

            return Mapper.Map<IQueryable<Device>, List<ApiDevice>>(deviceEntities);
        }

        public ApiDevice GetById(Guid id)
        {
            var deviceEntity = UnitOfWork.GetRepository<Device>().GetById(id);

            return (deviceEntity != null) ? Mapper.Map<Device, ApiDevice>(deviceEntity) : null;
        }

        public Guid Save(ApiDevice model)
        {
            var deviceEntity = UnitOfWork.GetRepository<Device>().GetById(model.Id);

            if (deviceEntity == null)
            {
                deviceEntity = Mapper.Map<ApiDevice, Device>(model);

                UnitOfWork.GetRepository<Device>().Insert(deviceEntity);
            }
            else
            {
                deviceEntity = Mapper.Map(model, deviceEntity);

                UnitOfWork.GetRepository<Device>().Update(deviceEntity);
            }

            UnitOfWork.SaveChanges();

            return deviceEntity.Id;
        }

        public void Delete(ApiDevice model)
        {
            UnitOfWork.GetRepository<Device>().Delete(model.Id);
            UnitOfWork.SaveChanges();
        }

        public void Delete(Guid modelId)
        {
            UnitOfWork.GetRepository<Device>().Delete(modelId);
            UnitOfWork.SaveChanges();
        }

        public ApiDevice GetByToken(string token)
        {
            var deviceEntity = UnitOfWork.GetRepository<Device>().GetAll().SingleOrDefault(d => d.Token.Equals(token));

            return (deviceEntity != null) ? Mapper.Map<Device, ApiDevice>(deviceEntity) : null;
        }

        public List<ApiDevice> GetUserDevices(Guid userId)
        {
            var deviceEntities = UnitOfWork.GetRepository<Device>().GetAll().Where(d => d.UserId == userId);

            return Mapper.Map<IQueryable<Device>, List<ApiDevice>>(deviceEntities);
        }
    }
}