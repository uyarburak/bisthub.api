using System;

namespace BistHub.Api.Dtos
{
    public class UpdatePositionRequest : CreatePositionRequest
    {
        public DateTime? SellDate { get; set; }
        public decimal? SellPrice { get; set; }
    }
}
