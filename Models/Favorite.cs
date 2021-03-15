using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Models
{
    [Table("favorites")]
    public partial class Favorite
    {
        [Key]
        [Column("username")]
        [StringLength(32)]
        public string Username { get; set; }
        [Key]
        [Column("order_no")]
        public short OrderNo { get; set; }
        [Required]
        [Column("stock_code")]
        [StringLength(8)]
        public string StockCode { get; set; }

        [ForeignKey(nameof(StockCode))]
        [InverseProperty(nameof(Stock.Favorites))]
        public virtual Stock StockCodeNavigation { get; set; }
        [ForeignKey(nameof(Username))]
        [InverseProperty(nameof(User.Favorites))]
        public virtual User UsernameNavigation { get; set; }
    }
}
