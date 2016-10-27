using System.Collections.Generic;
using Ontext.Core.Objects;
using Ontext.Core.Responses.Base;

namespace Ontext.Core.Responses
{
    public class ApiMessageResponse : ApiBaseResponse
    {
        public List<ApiMessage> Items { get; set; }
    }
}