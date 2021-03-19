using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BistHub.Api.Models
{
    [Keyless]
    [Table("stock_price_last")]
    public partial class StockPriceLast
    {
        [Column("date", TypeName = "date")]
        public DateTime? Date { get; set; }
        [Column("stock_code")]
        [StringLength(8)]
        public string StockCode { get; set; }
        [Column("close")]
        public decimal? Close { get; set; }
        [Column("high")]
        public decimal? High { get; set; }
        [Column("low")]
        public decimal? Low { get; set; }
        [Column("open")]
        public decimal? Open { get; set; }
        [Column("i")]
        public long? I { get; set; }
        [Column("last_date", TypeName = "date")]
        public DateTime? LastDate { get; set; }
        [Column("last_close")]
        public decimal? LastClose { get; set; }
    }
}
