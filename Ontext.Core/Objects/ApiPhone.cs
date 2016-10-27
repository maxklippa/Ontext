using System;
using Ontext.Core.Objects.Base;

namespace Ontext.Core.Objects
{
    public class ApiPhone : ApiEntity
    {
        public string Number { get; set; }
        public int Priority { get; set; }
        public Guid? UserId { get; set; }

        public override string ToString()
        {
            return Number;
        }
    }
}