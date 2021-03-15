using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Models;

#nullable disable

namespace BistHub.Data
{
    public partial class BistHubContext : DbContext
    {
        public BistHubContext()
        {
        }

        public BistHubContext(DbContextOptions<BistHubContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<List> Lists { get; set; }
        public virtual DbSet<Portfolio> Portfolios { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockList> StockLists { get; set; }
        public virtual DbSet<StockPrice> StockPrices { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp")
                .HasAnnotation("Relational:Collation", "en_US.utf8");

            modelBuilder.Entity<Favorite>(entity =>
            {
                entity.HasKey(e => new { e.Username, e.OrderNo })
                    .HasName("favorites_pkey");

                entity.HasOne(d => d.StockCodeNavigation)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.StockCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("favorites_stock_code_fkey");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Favorites)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("favorites_username_fkey");
            });

            modelBuilder.Entity<List>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("lists_pkey");
            });

            modelBuilder.Entity<Portfolio>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.Portfolios)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("portfolios_username_fkey");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.BuyDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.BuyPrice).HasPrecision(10, 4);

                entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.PaidFee).HasPrecision(9, 2);

                entity.Property(e => e.SellPrice).HasPrecision(10, 4);

                entity.Property(e => e.Updated).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Portfolio)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.PortfolioId)
                    .HasConstraintName("positions_portfolio_id_fkey");

                entity.HasOne(d => d.StockCodeNavigation)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.StockCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("positions_stock_code_fkey");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("stocks_pkey");
            });

            modelBuilder.Entity<StockList>(entity =>
            {
                entity.HasKey(e => new { e.StockCode, e.ListCode })
                    .HasName("stock_list_pkey");

                entity.HasOne(d => d.ListCodeNavigation)
                    .WithMany(p => p.StockLists)
                    .HasForeignKey(d => d.ListCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("stock_list_list_code_fkey");

                entity.HasOne(d => d.StockCodeNavigation)
                    .WithMany(p => p.StockLists)
                    .HasForeignKey(d => d.StockCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("stock_list_stock_code_fkey");
            });

            modelBuilder.Entity<StockPrice>(entity =>
            {
                entity.HasKey(e => new { e.Date, e.StockCode })
                    .HasName("stock_price_pkey");

                entity.Property(e => e.Close).HasPrecision(10, 4);

                entity.Property(e => e.High).HasPrecision(10, 4);

                entity.Property(e => e.Low).HasPrecision(10, 4);

                entity.Property(e => e.Open).HasPrecision(10, 4);

                entity.HasOne(d => d.StockCodeNavigation)
                    .WithMany(p => p.StockPrices)
                    .HasForeignKey(d => d.StockCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("stock_price_stock_code_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("users_pkey");

                entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.EmailVerified).HasPrecision(1);

                entity.Property(e => e.PhoneVerified).HasPrecision(1);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
