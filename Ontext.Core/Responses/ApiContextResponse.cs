using System.Collections.Generic;
using Ontext.Core.Objects;
using Ontext.Core.Responses.Base;

namespace Ontext.Core.Responses
{
    public class ApiContextResponse : ApiBaseResponse
    {
        public List<ApiContext> Items { get; set; }
    }
}