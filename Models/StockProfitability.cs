using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace BistHub.Api.Models
{
    [Table("stock_profitabilities")]
    public partial class StockProfitability
    {
        [Key]
        [Column("stock_code")]
        [StringLength(8)]
        public string StockCode { get; set; }
        [Key]
        [Column("period")]
        [StringLength(7)]
        public string Period { get; set; }
        [Column("gross_profit_margin_quarterly")]
        public decimal? GrossProfitMarginQuarterly { get; set; }
        [Column("gross_profit_margin_annualized")]
        public decimal? GrossProfitMarginAnnualized { get; set; }
        [Column("operating_expenses_margin_quarterly")]
        public decimal? OperatingExpensesMarginQuarterly { get; set; }
        [Column("operating_expenses_margin_annualized")]
        public decimal? OperatingExpensesMarginAnnualized { get; set; }
        [Column("ebitda_margin_quarterly")]
        public decimal? EbitdaMarginQuarterly { get; set; }
        [Column("ebitda_margin_annualized")]
        public decimal? EbitdaMarginAnnualized { get; set; }
        [Column("net_profit_margin_quarterly")]
        public decimal? NetProfitMarginQuarterly { get; set; }
        [Column("net_profit_margin_annualized")]
        public decimal? NetProfitMarginAnnualized { get; set; }
        [Column("roe")]
        public decimal? Roe { get; set; }
        [Column("roa")]
        public decimal? Roa { get; set; }
        [Column("net_debts_to_ebitda")]
        public decimal? NetDebtsToEbitda { get; set; }
        [Column("net_financial_incomes_to_ebitda")]
        public decimal? NetFinancialIncomesToEbitda { get; set; }
        [Column("net_debts_to_equity")]
        public decimal? NetDebtsToEquity { get; set; }
        [Column("created")]
        public DateTime Created { get; set; }

        [ForeignKey(nameof(StockCode))]
        [InverseProperty(nameof(Stock.StockProfitabilities))]
        public virtual Stock StockCodeNavigation { get; set; }
    }
}
