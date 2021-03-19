using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BistHub.Api.Models
{
    [Table("users")]
    public partial class User
    {
        public User()
        {
            Favorites = new HashSet<Favorite>();
            Portfolios = new HashSet<Portfolio>();
        }

        [Key]
        [Column("username")]
        [StringLength(32)]
        public string Username { get; set; }
        [Required]
        [Column("email")]
        [StringLength(128)]
        public string Email { get; set; }
        [Required]
        [Column("phone")]
        [StringLength(14)]
        public string Phone { get; set; }
        [Required]
        [Column("password")]
        [StringLength(255)]
        public string Password { get; set; }
        [Column("email_verified")]
        public bool EmailVerified { get; set; }
        [Column("phone_verified")]
        public bool PhoneVerified { get; set; }
        [Required]
        [Column("first_name")]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Required]
        [Column("last_name")]
        [StringLength(255)]
        public string LastName { get; set; }
        [Column("is_admin")]
        public bool IsAdmin { get; set; }
        [Required]
        [Column("locale")]
        [StringLength(6)]
        public string Locale { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }

        [InverseProperty(nameof(Favorite.UsernameNavigation))]
        public virtual ICollection<Favorite> Favorites { get; set; }
        [InverseProperty(nameof(Portfolio.UsernameNavigation))]
        public virtual ICollection<Portfolio> Portfolios { get; set; }
    }
}
