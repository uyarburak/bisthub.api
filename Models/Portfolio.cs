using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BistHub.Api.Models
{
    [Table("portfolios")]
    public partial class Portfolio
    {
        public Portfolio()
        {
            Positions = new HashSet<Position>();
        }

        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("title")]
        [StringLength(32)]
        public string Title { get; set; }
        [Column("username")]
        [StringLength(32)]
        public string Username { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }

        [ForeignKey(nameof(Username))]
        [InverseProperty(nameof(User.Portfolios))]
        public virtual User UsernameNavigation { get; set; }
        [InverseProperty(nameof(Position.Portfolio))]
        public virtual ICollection<Position> Positions { get; set; }
    }
}
