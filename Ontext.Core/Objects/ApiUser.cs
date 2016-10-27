using System;
using System.Collections.Generic;
using Ontext.Core.Objects.Base;

namespace Ontext.Core.Objects
{
    public class ApiUser : ApiEntity
    {
        public string Email { get; set; }
        public List<Guid> ContactsId { get; set; }
        public List<Guid> PhonesId { get; set; }
        public List<Guid> DevicesId { get; set; }
    }
}