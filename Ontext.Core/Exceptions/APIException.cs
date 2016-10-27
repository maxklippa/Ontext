using System;
using Ontext.Core.Enums;

namespace Ontext.Core.Exceptions
{
    public class APIException : Exception
    {
        public APIExceptionType APIExceptionType = APIExceptionType.Unknown;

        public APIException()
        {
        }

        public APIException(string message)
            : base(message)
        {
        }

        public APIException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public APIException(APIExceptionType type, string message)
            : base(message)
        {
            APIExceptionType = type;
        }

        public APIException(APIExceptionType type, string message, Exception inner)
            : base(message, inner)
        {
            APIExceptionType = type;
        }

    }
}