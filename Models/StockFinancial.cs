using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BistHub.Api.Models
{
    [Table("stock_financials")]
    public partial class StockFinancial
    {
        [Key]
        [Column("stock_code")]
        [StringLength(8)]
        public string StockCode { get; set; }
        [Key]
        [Column("period")]
        [StringLength(7)]
        public string Period { get; set; }
        [Column("net_sales_quarterly")]
        public decimal? NetSalesQuarterly { get; set; }
        [Column("net_sales_annualized")]
        public decimal? NetSalesAnnualized { get; set; }
        [Column("ebitda_quarterly")]
        public decimal? EbitdaQuarterly { get; set; }
        [Column("ebitda_annualized")]
        public decimal? EbitdaAnnualized { get; set; }
        [Column("net_financial_incomes_quarterly")]
        public decimal? NetFinancialIncomesQuarterly { get; set; }
        [Column("net_financial_incomes_annualized")]
        public decimal? NetFinancialIncomesAnnualized { get; set; }
        [Column("net_profit_quarterly")]
        public decimal? NetProfitQuarterly { get; set; }
        [Column("net_profit_annualized")]
        public decimal? NetProfitAnnualized { get; set; }
        [Column("total_debts")]
        public decimal? TotalDebts { get; set; }
        [Column("net_debts")]
        public decimal? NetDebts { get; set; }
        [Column("net_working_capital")]
        public decimal? NetWorkingCapital { get; set; }
        [Column("equity")]
        public decimal? Equity { get; set; }
        [Column("assets")]
        public decimal? Assets { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }

        [ForeignKey(nameof(StockCode))]
        [InverseProperty(nameof(Stock.StockFinancials))]
        public virtual Stock StockCodeNavigation { get; set; }
    }
}
