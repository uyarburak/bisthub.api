using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Models
{
    [Table("stock_price")]
    public partial class StockPrice
    {
        [Key]
        [Column("date", TypeName = "date")]
        public DateTime Date { get; set; }
        [Key]
        [Column("stock_code")]
        [StringLength(8)]
        public string StockCode { get; set; }
        [Column("close")]
        public decimal Close { get; set; }
        [Column("high")]
        public decimal High { get; set; }
        [Column("low")]
        public decimal Low { get; set; }
        [Column("open")]
        public decimal Open { get; set; }

        [ForeignKey(nameof(StockCode))]
        [InverseProperty(nameof(Stock.StockPrices))]
        public virtual Stock StockCodeNavigation { get; set; }
    }
}
