using Ontext.DAL.Identity;
using Ontext.DAL.Models.Base;
using System;

namespace Ontext.DAL.Models
{
    public class Device : BaseEntity
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }

        public virtual OntextUser User { get; set; }
    }
}
