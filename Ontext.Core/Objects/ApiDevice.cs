using System;
using System.Collections.Generic;
using Ontext.Core.Objects.Base;

namespace Ontext.Core.Objects
{
    public class ApiDevice : ApiEntity
    {
        public string Token { get; set; }
        public Guid UserId { get; set; }
    }
}
