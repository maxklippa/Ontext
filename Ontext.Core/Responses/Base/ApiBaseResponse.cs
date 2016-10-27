using System;
using Ontext.Core.Enums;

namespace Ontext.Core.Responses.Base
{

    /// <summary>
    /// Base APi response calss
    /// </summary>
    public class ApiBaseResponse
    {
        /// <summary>
        /// Api status code
        /// </summary>
        public ApiStatusCode Status { get; set; }
        /// <summary>
        /// Api error message
        /// </summary>
        public String Error { get; set; }
    }
}
