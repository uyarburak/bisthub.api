using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BistHub.Api.Models
{
    [Table("positions")]
    public partial class Position
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("portfolio_id")]
        public Guid? PortfolioId { get; set; }
        [Required]
        [Column("stock_code")]
        [StringLength(8)]
        public string StockCode { get; set; }
        [Column("amount")]
        public long Amount { get; set; }
        [Column("buy_date", TypeName = "date")]
        public DateTime BuyDate { get; set; }
        [Column("buy_price")]
        public decimal BuyPrice { get; set; }
        [Column("sell_date", TypeName = "date")]
        public DateTime? SellDate { get; set; }
        [Column("sell_price")]
        public decimal? SellPrice { get; set; }
        [Column("paid_fee")]
        public decimal PaidFee { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }
        [Column("updated")]
        public DateTime Updated { get; set; }

        [ForeignKey(nameof(PortfolioId))]
        [InverseProperty("Positions")]
        public virtual Portfolio Portfolio { get; set; }
        [ForeignKey(nameof(StockCode))]
        [InverseProperty(nameof(Stock.Positions))]
        public virtual Stock StockCodeNavigation { get; set; }
    }
}
