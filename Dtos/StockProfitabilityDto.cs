namespace BistHub.Api.Dtos
{
    public class StockProfitabilityDto
    {
        public string Period { get; set; }
        public decimal? GrossProfitMarginQuarterly { get; set; }
        public decimal? GrossProfitMarginAnnualized { get; set; }
        public decimal? OperatingExpensesMarginQuarterly { get; set; }
        public decimal? OperatingExpensesMarginAnnualized { get; set; }
        public decimal? EbitdaMarginQuarterly { get; set; }
        public decimal? EbitdaMarginAnnualized { get; set; }
        public decimal? NetProfitMarginQuarterly { get; set; }
        public decimal? NetProfitMarginAnnualized { get; set; }
        public decimal? Roe { get; set; }
        public decimal? Roa { get; set; }
        public decimal? NetDebtsToEbitda { get; set; }
        public decimal? NetFinancialIncomesToEbitda { get; set; }
        public decimal? NetDebtsToEquity { get; set; }
    }
}
