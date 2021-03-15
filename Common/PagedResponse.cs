using System.Collections.Generic;

namespace BistHub.Api.Common
{
    public class PagedResponse<T> : BaseListResponse<T>
    {
        public long TotalItems { get; set; }

        public static PagedResponse<T> Successful(long totalItems, IEnumerable<T> list, string infoMessage = null)
        {
            return new PagedResponse<T>
            {
                Success = true,
                Message = infoMessage,
                Data = list,
                TotalItems = totalItems
            };
        }
    }
}
