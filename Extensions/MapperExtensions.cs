using BistHub.Api.Dtos;
using Models;

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
    }
}
