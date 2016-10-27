using System;
using Ontext.Core.Objects.Base;

namespace Ontext.Core.Objects
{
    public class ApiSettings : ApiEntity
    {
        public int SortType { get; set; }
        public string Language { get; set; }
    }
}
