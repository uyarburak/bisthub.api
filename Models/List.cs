using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Models
{
    [Table("lists")]
    public partial class List
    {
        public List()
        {
            StockLists = new HashSet<StockList>();
        }

        [Key]
        [Column("code")]
        [StringLength(64)]
        public string Code { get; set; }
        [Required]
        [Column("title")]
        [StringLength(255)]
        public string Title { get; set; }
        [Column("description")]
        [StringLength(1024)]
        public string Description { get; set; }

        [InverseProperty(nameof(StockList.ListCodeNavigation))]
        public virtual ICollection<StockList> StockLists { get; set; }
    }
}
