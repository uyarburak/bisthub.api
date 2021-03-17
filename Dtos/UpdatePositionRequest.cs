using System;

namespace BistHub.Api.Dtos
{
    public class UpdatePositionRequest
    {
        public long Amount { get; set; }
        public decimal PaidFee { get; set; }
        public DateTime BuyDate { get; set; }
        public decimal BuyPrice { get; set; }
        public DateTime? SellDate { get; set; }
        public decimal? SellPrice { get; set; }
    }
}
