using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BistHub.Api.Models
{
    [Keyless]
    [Table("stock_price_view")]
    public partial class StockPriceView
    {
        [Column("code")]
        [StringLength(8)]
        public string Code { get; set; }
        [Column("min", TypeName = "date")]
        public DateTime? Min { get; set; }
        [Column("max", TypeName = "date")]
        public DateTime? Max { get; set; }
    }
}
