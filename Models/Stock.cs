using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Models
{
    [Table("stocks")]
    public partial class Stock
    {
        public Stock()
        {
            Favorites = new HashSet<Favorite>();
            Positions = new HashSet<Position>();
            StockLists = new HashSet<StockList>();
            StockPrices = new HashSet<StockPrice>();
        }

        [Key]
        [Column("code")]
        [StringLength(8)]
        public string Code { get; set; }
        [Required]
        [Column("company_name")]
        [StringLength(255)]
        public string CompanyName { get; set; }
        [Column("logo")]
        [StringLength(255)]
        public string Logo { get; set; }
        [Column("website")]
        [StringLength(255)]
        public string Website { get; set; }
        [Column("kap_url")]
        [StringLength(255)]
        public string KapUrl { get; set; }
        [Column("investing_url")]
        [StringLength(255)]
        public string InvestingUrl { get; set; }

        [InverseProperty(nameof(Favorite.StockCodeNavigation))]
        public virtual ICollection<Favorite> Favorites { get; set; }
        [InverseProperty(nameof(Position.StockCodeNavigation))]
        public virtual ICollection<Position> Positions { get; set; }
        [InverseProperty(nameof(StockList.StockCodeNavigation))]
        public virtual ICollection<StockList> StockLists { get; set; }
        [InverseProperty(nameof(StockPrice.StockCodeNavigation))]
        public virtual ICollection<StockPrice> StockPrices { get; set; }
    }
}
