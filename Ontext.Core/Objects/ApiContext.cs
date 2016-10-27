using System;
using System.Collections.Generic;
using Ontext.Core.Objects.Base;

namespace Ontext.Core.Objects
{
    public class ApiContext : ApiEntity
    {
        public string Name { get; set; }
        public List<Guid> ContactsId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}