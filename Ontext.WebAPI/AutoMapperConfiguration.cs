using System;
using System.Linq;
using AutoMapper;
using Ontext.Core.Enums;
using Ontext.Core.Objects;
using Ontext.DAL.Identity;
using Ontext.DAL.Models;

namespace Ontext.WebAPI
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            ConfigureClientMapping();
        }

        private static void ConfigureClientMapping()
        {
            //Entity to DTO
            Mapper.CreateMap<OntextUser, ApiUser>()
                .ForMember(d => d.ContactsId, o => o.MapFrom(src => src.Contacts.Select(x => x.Id)))
                .ForMember(d => d.PhonesId, o => o.MapFrom(src => src.Phones.Select(x => x.Id)))
                .ForMember(d => d.DevicesId, o => o.MapFrom(src => src.Devices.Select(x => x.Id)))
                .IgnoreAllNonExisting();
            Mapper.CreateMap<Contact, ApiContact>()
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(src => src.Phone != null ? src.Phone.Number : null))
                .IgnoreAllNonExisting();
            Mapper.CreateMap<Context, ApiContext>().IgnoreAllNonExisting();
            Mapper.CreateMap<Message, ApiMessage>()
                .ForMember(d => d.ContactId, o => o.MapFrom(src => (src.Contact != null) ? src.ContactId : Guid.Empty))
                .IgnoreAllNonExisting();
            Mapper.CreateMap<Phone, ApiPhone>().IgnoreAllNonExisting();
            Mapper.CreateMap<Device, ApiDevice>().IgnoreAllNonExisting();
            Mapper.CreateMap<Settings, ApiSettings>().IgnoreAllNonExisting();

            //DTO to Entity
            Mapper.CreateMap<ApiUser, OntextUser>().IgnoreAllNonExisting();
            Mapper.CreateMap<ApiContact, Contact>().IgnoreAllNonExisting();
            Mapper.CreateMap<ApiContext, Context>().IgnoreAllNonExisting();
            Mapper.CreateMap<ApiMessage, Message>().IgnoreAllNonExisting();
            Mapper.CreateMap<ApiPhone, Phone>().IgnoreAllNonExisting();
            Mapper.CreateMap<ApiDevice, Device>().IgnoreAllNonExisting();
            Mapper.CreateMap<ApiSettings, Settings>().IgnoreAllNonExisting();
        }
    }
}