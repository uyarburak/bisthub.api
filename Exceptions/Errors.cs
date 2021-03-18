namespace BistHub.Api.Exceptions
{
    public static class Errors
    {
        public const string PortfolioNotCreated = "PORTFOLIO_NOT_CREATED";
        public const string PortfolioTitleNotUpdated = "PORTFOLIO_TITLE_NOT_UPDATED";
        public const string PositionNotCreated = "POSITION_NOT_CREATED";
        public const string PositionNotFound = "POSITION_NOT_FOUND";
        public const string PortfolioNotFound = "PORTFOLIO_NOT_FOUND";
        public const string NotOwnThePortfolio = "NOT_OWN_THE_PORTFOLIO";
        public const string PositionNotRemoved = "POSITION_NOT_REMOVED";
        public const string PositionNotUpdated = "POSITION_NOT_UPDATED";
        public const string StockNotFound = "STOCK_NOT_FOUND";

        public const string InvalidPortfolioTitle = "INVALID_PORTFOLIO_TITLE";
        public const string PositionAmountMustBePositive = "POSITION_AMOUNT_MUST_BE_POSITIVE";
        public const string PositionBuyDateMustPassed = "POSITION_BUY_DATE_MUST_PASSED";
        public const string PositionBuyPriceMustBePositive = "POSITION_BUY_PRICE_MUST_BE_POSITIVE";
        public const string PositionPaidFeeCannotBeNegative = "POSITION_PAID_FEE_CANNOT_BE_NEGATIVE";
        public const string PositionSellDateMustPassed = "POSITION_SELL_DATE_MUST_PASSED";
        public const string PositionSellPriceMustBePositive = "POSITION_SELL_PRICE_MUST_BE_POSITIVE";
        public const string PositionSellPriceAndDateNotFound = "POSITION_SELL_PRICE_AND_DATE_NOT_FOUND";
    }
}
