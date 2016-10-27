using System.Collections.Generic;
using Ontext.Core.Objects;
using Ontext.Core.Responses.Base;

namespace Ontext.Core.Responses
{
    public class ApiUserResponse : ApiBaseResponse
    {
        public List<ApiUser> Items { get; set; }
    }
}