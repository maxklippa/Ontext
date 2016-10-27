using System.Collections.Generic;
using Ontext.Core.Objects;
using Ontext.Core.Responses.Base;

namespace Ontext.Core.Responses
{
    public class ApiPhoneResponse : ApiBaseResponse
    {
        public List<ApiPhone> Items { get; set; }
    }
}