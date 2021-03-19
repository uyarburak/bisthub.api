using BistHub.Api.Dtos;
using BistHub.Api.Models;

namespace BistHub.Api.Extensions
{
    public static class MapperExtensions
    {
        public static StockDto ToStockDto(this Stock stock)
        {
            return new StockDto
            {
                Code = stock.Code,
                CompanyName = stock.CompanyName,
                Logo = stock.Logo,
                Website = stock.Website,
                KapUrl = stock.KapUrl,
                InvestingUrl = stock.InvestingUrl
            };
        }

        public static PortfolioDto ToPortfolioDto(this Portfolio portfolio)
        {
            return new PortfolioDto
            {
                Id = portfolio.Id.ToString(),
                Title = portfolio.Title,
                Created = portfolio.Created
            };
        }

        public static PositionDto ToPositionDto(this Position position)
        {
            return new PositionDto
            {
                Id = position.Id,
                StockCode = position.StockCode,
                Amount = position.Amount,
                BuyDate = position.BuyDate,
                BuyPrice = position.BuyPrice,
                SellDate = position.SellDate,
                SellPrice = position.SellPrice,
                PaidFee = position.PaidFee,
                Created = position.Created,
                Updated = position.Updated
            };
        }

        public static StockFinancialDto ToStockFinancialDto(this StockFinancial stockFinancial)
        {
            return new StockFinancialDto
            {
                Assets = stockFinancial.Assets,
                EbitdaAnnualized = stockFinancial.EbitdaAnnualized,
                EbitdaQuarterly = stockFinancial.EbitdaQuarterly,
                Equity = stockFinancial.Equity,
                NetDebts = stockFinancial.NetDebts,
                NetFinancialIncomesAnnualized = stockFinancial.NetFinancialIncomesAnnualized,
                NetFinancialIncomesQuarterly = stockFinancial.NetFinancialIncomesQuarterly,
                NetProfitAnnualized = stockFinancial.NetProfitAnnualized,
                NetProfitQuarterly = stockFinancial.NetProfitQuarterly,
                NetSalesAnnualized = stockFinancial.NetSalesAnnualized,
                NetSalesQuarterly = stockFinancial.NetSalesQuarterly,
                NetWorkingCapital = stockFinancial.NetWorkingCapital,
                Period = stockFinancial.Period,
                TotalDebts = stockFinancial.TotalDebts
            };
        }

        public static StockProfitabilityDto ToStockProfitabilityDto(this StockProfitability profitability)
        {
            return new StockProfitabilityDto
            {
                EbitdaMarginAnnualized = profitability.EbitdaMarginAnnualized,
                EbitdaMarginQuarterly = profitability.EbitdaMarginQuarterly,
                GrossProfitMarginAnnualized = profitability.GrossProfitMarginAnnualized,
                GrossProfitMarginQuarterly = profitability.GrossProfitMarginQuarterly,
                NetDebtsToEbitda = profitability.NetDebtsToEbitda,
                NetDebtsToEquity = profitability.NetDebtsToEquity,
                NetFinancialIncomesToEbitda = profitability.NetFinancialIncomesToEbitda,
                NetProfitMarginAnnualized = profitability.NetProfitMarginAnnualized,
                NetProfitMarginQuarterly = profitability.NetProfitMarginQuarterly,
                OperatingExpensesMarginAnnualized = profitability.OperatingExpensesMarginAnnualized,
                OperatingExpensesMarginQuarterly = profitability.OperatingExpensesMarginQuarterly,
                Period = profitability.Period,
                Roa = profitability.Roa,
                Roe = profitability.Roe
            };
        }
    }
}
