using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ontext.DAL.Identity;
using Ontext.DAL.Models.Base;

namespace Ontext.DAL.Models
{
    public class Phone : BaseEntity
    {
        [Index("PhoneNumberIndex", IsUnique = true)]
        [StringLength(100)]
        public string Number { get; set; }
        
        public int Priority { get; set; }
        public Guid? UserId { get; set; }

        public virtual OntextUser User { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
