using System.Collections.Generic;
using Ontext.Core.Objects;
using Ontext.Core.Responses.Base;

namespace Ontext.Core.Responses
{
    public class ApiContactResponse : ApiBaseResponse
    {
        public List<ApiContact> Items { get; set; }
    }
}