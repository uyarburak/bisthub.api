using System;

namespace BistHub.Api.Dtos
{
    public class CreatePositionRequest
    {
        public string StockCode { get; set; }
        public long Amount { get; set; }
        public decimal PaidFee { get; set; }
        public DateTime BuyDate { get; set; }
        public decimal BuyPrice { get; set; }
    }
}
