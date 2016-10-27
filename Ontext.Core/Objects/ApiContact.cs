using System;
using Ontext.Core.Objects.Base;

namespace Ontext.Core.Objects
{
    public class ApiContact : ApiEntity
    {
        public string Name { get; set; }
        public bool Blocked { get; set; }
        public string PhoneNumber { get; set; }
        public int HasApp {	get; set; }
        public Guid UserId { get; set; }
        public Guid ContextId { get; set; }
        public Guid PhoneId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}