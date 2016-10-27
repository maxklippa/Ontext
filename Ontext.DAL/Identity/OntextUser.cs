using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ontext.DAL.Models;

namespace Ontext.DAL.Identity
{
    public class OntextUser : IdentityUser<Guid, OntextUserLogin, OntextUserRole,OntextUserClaim>
    {
        public OntextUser()
        {
            Id = Guid.NewGuid();
            Contacts = new Collection<Contact>();
            Phones = new Collection<Phone>();
            Devices = new Collection<Device>();
            Messages = new Collection<Message>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<OntextUser, Guid> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
        
        public virtual Settings Settings { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}