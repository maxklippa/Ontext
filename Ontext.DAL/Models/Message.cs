using Ontext.Core.Enums;
using Ontext.DAL.Identity;
using Ontext.DAL.Models.Base;
using System;

namespace Ontext.DAL.Models
{
    public class Message : BaseEntity
    {
        public string Text { get; set; }
        public string Image { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public Guid UserId { get; set; }
        public bool Read { get; set; }
        public DateTime CreationDate { get; set; }
        public MessageType Type { get; set; }
        public string SenderName { get; set; }
        public Guid ContactId { get; set; }
        public Guid ContextId { get; set; }
    
        public virtual Contact Contact { get; set; }
        public virtual Context Context { get; set; }
        public virtual OntextUser User { get; set; }
    }
}
