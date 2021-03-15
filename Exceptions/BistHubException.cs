using System;

namespace BistHub.Api.Exceptions
{
    public class BistHubException : Exception
    {
        public int HttpCode { get; private set; }
        public string ErrorCode { get; private set; }

        public BistHubException(int httpCode, string errorCode, string errorMessage, Exception? innerException = null) : base(errorMessage, innerException)
        {
            HttpCode = httpCode;
            ErrorCode = errorCode;
        }
    }
}
