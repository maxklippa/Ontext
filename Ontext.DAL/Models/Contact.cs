using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using Ontext.DAL.Identity;
using Ontext.DAL.Models.Base;
using System.Collections.Generic;

namespace Ontext.DAL.Models
{
    public class Contact : BaseEntity
    {
        public string Name { get; set; }
        public bool Blocked { get; set; }
        public Guid UserId { get; set; }
        public Guid ContextId { get; set; }
        public Guid PhoneId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime LastModifiedDate { get; set; }
        [Required]
        public bool IsDeleted { get; set; }

        public virtual OntextUser User { get; set; }
        public virtual Context Context { get; set; }
        public virtual Phone Phone { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
