namespace BistHub.Api.Common
{
    public class BaseResponse<T> : BaseResponse
    {
        public T Data { get; set; }
        public static BaseResponse<T> Successful(T data, string infoMessage = null)
        {
            return new BaseResponse<T>
            {
                Success = true,
                Message = infoMessage,
                Data = data
            };
        }

        public static new BaseResponse<T> Fail(string errorCode, string message = null)
        {
            return new BaseResponse<T>
            {
                ErrorCode = errorCode,
                Message = message
            };
        }
    }

    public class BaseResponse
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }

        public static BaseResponse Successful(string infoMessage = null)
        {
            return new BaseResponse
            {
                Success = true,
                Message = infoMessage
            };
        }

        public static BaseResponse Fail(string errorCode, string message = null)
        {
            return new BaseResponse
            {
                ErrorCode = errorCode,
                Message = message
            };
        }
    }
}
