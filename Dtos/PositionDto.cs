using System;

namespace BistHub.Api.Dtos
{
    public class PositionDto
    {
        public int Id { get; set; }
        public string StockCode { get; set; }
        public long Amount { get; set; }
        public DateTime BuyDate { get; set; }
        public decimal BuyPrice { get; set; }
        public DateTime? SellDate { get; set; }
        public decimal? SellPrice { get; set; }
        public decimal PaidFee { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
