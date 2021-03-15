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

        public static new BaseResponse<T> Fail(string message)
        {
            return Fail(new ErrorModel(null, message));
        }

        public static new BaseResponse<T> Fail(ErrorModel error)
        {
            return new BaseResponse<T>
            {
                Success = false,
                Message = error.Description,
                Error = error
            };
        }
    }

    public class BaseResponse
    {
        public bool Success { get; set; }
        public ErrorModel Error { get; set; }
        public string Message { get; set; }

        public static BaseResponse Fail(string message)
        {
            return Fail(new ErrorModel(null, message));
        }

        public static BaseResponse Fail(ErrorModel error)
        {
            return new BaseResponse
            {
                Success = false,
                Message = error.Description,
                Error = error
            };
        }

        public static BaseResponse Successful(string infoMessage = null)
        {
            return new BaseResponse
            {
                Success = true,
                Message = infoMessage
            };
        }
    }

    public class ErrorModel
    {
        public ErrorModel(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; set; }
        public string Description { get; set; }
    }
}
