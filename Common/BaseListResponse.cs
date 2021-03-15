using System.Collections.Generic;
using System.Linq;

namespace BistHub.Api.Common
{
    public class BaseListResponse<T> : BaseResponse<IEnumerable<T>>
    {
        public int Count => Data.Count();

        public static new BaseListResponse<T> Successful(IEnumerable<T> data, string infoMessage = null)
        {
            return new BaseListResponse<T>
            {
                Success = true,
                Message = infoMessage,
                Data = data
            };
        }
    }
}
