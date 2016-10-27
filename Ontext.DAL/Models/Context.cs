using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ontext.DAL.Models.Base;
using System.Collections.Generic;

namespace Ontext.DAL.Models
{
    public class Context : BaseEntity
    {
        [Index("ContextNameIndex", IsUnique = true)]
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime LastModifiedDate { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
    
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}
