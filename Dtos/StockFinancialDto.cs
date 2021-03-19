namespace BistHub.Api.Dtos
{
    public class StockFinancialDto
    {
        public string Period { get; set; }
        public decimal? NetSalesQuarterly { get; set; }
        public decimal? NetSalesAnnualized { get; set; }
        public decimal? EbitdaQuarterly { get; set; }
        public decimal? EbitdaAnnualized { get; set; }
        public decimal? NetFinancialIncomesQuarterly { get; set; }
        public decimal? NetFinancialIncomesAnnualized { get; set; }
        public decimal? NetProfitQuarterly { get; set; }
        public decimal? NetProfitAnnualized { get; set; }
        public decimal? TotalDebts { get; set; }
        public decimal? NetDebts { get; set; }
        public decimal? NetWorkingCapital { get; set; }
        public decimal? Equity { get; set; }
        public decimal? Assets { get; set; }
    }
}
