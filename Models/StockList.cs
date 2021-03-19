using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BistHub.Api.Models
{
    [Table("stock_list")]
    public partial class StockList
    {
        [Key]
        [Column("stock_code")]
        [StringLength(8)]
        public string StockCode { get; set; }
        [Key]
        [Column("list_code")]
        [StringLength(64)]
        public string ListCode { get; set; }

        [ForeignKey(nameof(ListCode))]
        [InverseProperty(nameof(List.StockLists))]
        public virtual List ListCodeNavigation { get; set; }
        [ForeignKey(nameof(StockCode))]
        [InverseProperty(nameof(Stock.StockLists))]
        public virtual Stock StockCodeNavigation { get; set; }
    }
}
