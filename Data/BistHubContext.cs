using Microsoft.EntityFrameworkCore;
using BistHub.Api.Models;

#nullable disable

namespace BistHub.Api.Data
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
        public virtual DbSet<StockFinancial> StockFinancials { get; set; }
        public virtual DbSet<StockList> StockLists { get; set; }
        public virtual DbSet<StockPrice> StockPrices { get; set; }
        public virtual DbSet<StockPriceLast> StockPriceLasts { get; set; }
        public virtual DbSet<StockPriceView> StockPriceViews { get; set; }
        public virtual DbSet<StockProfitability> StockProfitabilities { get; set; }
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

            modelBuilder.Entity<StockFinancial>(entity =>
            {
                entity.HasKey(e => new { e.StockCode, e.Period })
                    .HasName("stock_financials_pkey");

                entity.Property(e => e.Period).IsFixedLength(true);

                entity.Property(e => e.Assets)
                    .HasPrecision(19, 3)
                    .HasComment("Toplam Aktifler (Mln TL)");

                entity.Property(e => e.Created)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("Kaydın sisteme giriş tarihi");

                entity.Property(e => e.EbitdaAnnualized)
                    .HasPrecision(19, 3)
                    .HasComment("FAVÖK (Yıllıklandırılmış) (Mln TL): Şirketin ilgili dönemden geriye dönük 4 çeyreklik operasyonlarından elde ettiği kar rakamı toplamını ifade eder.");

                entity.Property(e => e.EbitdaQuarterly)
                    .HasPrecision(19, 3)
                    .HasComment("FAVÖK (Çeyrek) (Mln TL)");

                entity.Property(e => e.Equity)
                    .HasPrecision(19, 3)
                    .HasComment("Özkaynaklar (Mln TL)");

                entity.Property(e => e.NetDebts)
                    .HasPrecision(19, 3)
                    .HasComment("Net Borç  (Mln TL): Şirketin toplam borcundan nakitlerinin çıkarılmasıyla kalan rakamı ifade eder.");

                entity.Property(e => e.NetFinancialIncomesAnnualized)
                    .HasPrecision(19, 3)
                    .HasComment("Net Finansal Gelirler / Giderler (Yıllıklandırılmış) (Mln TL)");

                entity.Property(e => e.NetFinancialIncomesQuarterly)
                    .HasPrecision(19, 3)
                    .HasComment("Net Finansal Gelirler / Giderler (Çeyrek) (Mln TL)");

                entity.Property(e => e.NetProfitAnnualized)
                    .HasPrecision(19, 3)
                    .HasComment("Net Kâr (Yıllıklandırılmış) (Mln TL): Şirketin ilgili dönemden geriye dönük 4 çeyreklik net kar rakamı toplamını ifade eder.");

                entity.Property(e => e.NetProfitQuarterly)
                    .HasPrecision(19, 3)
                    .HasComment("Net Kâr Çeyrek (Mln TL)");

                entity.Property(e => e.NetSalesAnnualized)
                    .HasPrecision(19, 3)
                    .HasComment("Net Satışlar (Yıllıklandırılmış) (Mln TL): Şirketin ilgili dönemden geriye dönük 4 çeyreklik satış rakamı toplamını ifade eder.");

                entity.Property(e => e.NetSalesQuarterly)
                    .HasPrecision(19, 3)
                    .HasComment("Net Satışlar Çeyrek (Mln TL)");

                entity.Property(e => e.NetWorkingCapital)
                    .HasPrecision(19, 3)
                    .HasComment("Net İşletme Sermayesi (Mln TL): Ticari alacak ve stoklar toplamından ticari borçların çıkarılmasıyla bulunur. Dönemler arasında artış olduğu takdirde şirketin işletme sermayesini nakitle fonladığını ifade eder.");

                entity.Property(e => e.TotalDebts)
                    .HasPrecision(19, 3)
                    .HasComment("Toplam Borçlar (Mln TL)");

                entity.HasOne(d => d.StockCodeNavigation)
                    .WithMany(p => p.StockFinancials)
                    .HasForeignKey(d => d.StockCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("stock_financials_stock_code_fkey");
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

            modelBuilder.Entity<StockPriceLast>(entity =>
            {
                entity.Property(e => e.Close).HasPrecision(10, 4);

                entity.Property(e => e.High).HasPrecision(10, 4);

                entity.Property(e => e.LastClose).HasPrecision(10, 4);

                entity.Property(e => e.Low).HasPrecision(10, 4);

                entity.Property(e => e.Open).HasPrecision(10, 4);
            });

            modelBuilder.Entity<StockProfitability>(entity =>
            {
                entity.HasKey(e => new { e.StockCode, e.Period })
                    .HasName("stock_profitabilities_pkey");

                entity.Property(e => e.Period).IsFixedLength(true);

                entity.Property(e => e.Created)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("Kaydın sisteme giriş tarihi");

                entity.Property(e => e.EbitdaMarginAnnualized)
                    .HasPrecision(5, 2)
                    .HasComment("FAVÖK Marjı (%) (Yıllıklandırılmış)");

                entity.Property(e => e.EbitdaMarginQuarterly)
                    .HasPrecision(5, 2)
                    .HasComment("FAVÖK Marjı (%) (Çeyrek)");

                entity.Property(e => e.GrossProfitMarginAnnualized)
                    .HasPrecision(5, 2)
                    .HasComment("Brüt Kâr Marjı (%) (Yıllıklandırılmış)");

                entity.Property(e => e.GrossProfitMarginQuarterly)
                    .HasPrecision(5, 2)
                    .HasComment("Brüt Kâr Marjı (%) (Çeyrek)");

                entity.Property(e => e.NetDebtsToEbitda)
                    .HasPrecision(5, 2)
                    .HasComment("Net Borç / FAVÖK (x)");

                entity.Property(e => e.NetDebtsToEquity)
                    .HasPrecision(5, 2)
                    .HasComment("Net Borç / Özsermaye (%)");

                entity.Property(e => e.NetFinancialIncomesToEbitda)
                    .HasPrecision(5, 2)
                    .HasComment("Net Finansman Gelir (Gider) / FAVÖK (x)");

                entity.Property(e => e.NetProfitMarginAnnualized)
                    .HasPrecision(5, 2)
                    .HasComment("Net Kâr Marjı (%) (Yıllıklandırılmış)");

                entity.Property(e => e.NetProfitMarginQuarterly)
                    .HasPrecision(5, 2)
                    .HasComment("Net Kâr Marjı (%) (Çeyreklik)");

                entity.Property(e => e.OperatingExpensesMarginAnnualized)
                    .HasPrecision(5, 2)
                    .HasComment("Faaliyet Giderleri Marjı (%) (Yıllıklandırılmış)");

                entity.Property(e => e.OperatingExpensesMarginQuarterly)
                    .HasPrecision(5, 2)
                    .HasComment("Faaliyet Giderleri Marjı (%) (Çeyrek)");

                entity.Property(e => e.Roa)
                    .HasPrecision(5, 2)
                    .HasComment("Aktif Kârlılık (%)");

                entity.Property(e => e.Roe)
                    .HasPrecision(5, 2)
                    .HasComment("Özsermaye Kârlılığı (%) (Yıllık)");

                entity.HasOne(d => d.StockCodeNavigation)
                    .WithMany(p => p.StockProfitabilities)
                    .HasForeignKey(d => d.StockCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("stock_profitabilities_stock_code_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("users_pkey");

                entity.Property(e => e.Created).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
