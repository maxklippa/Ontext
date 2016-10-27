using System.Collections.Generic;
using Ontext.Core.Objects;
using Ontext.Core.Responses.Base;

namespace Ontext.Core.Responses
{
    public class ApiSettingsResponse : ApiBaseResponse
    {
        public List<ApiSettings> Items { get; set; }
    }
}
