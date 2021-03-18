using System;

namespace BistHub.Api.Exceptions
{
    public class ValidationException : BistHubException
    {
        public ValidationException(string errorCode, string errorMessage, Exception? innerException = null) : base(400, errorCode, errorMessage, innerException)
        {
        }
    }
}
