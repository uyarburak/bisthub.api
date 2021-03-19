using BistHub.Api.Common;
using BistHub.Api.Data;
using BistHub.Api.Dtos;
using BistHub.Api.Exceptions;
using BistHub.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BistHub.Api.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BistHub.Api.Services;
using System;

namespace BistHub.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly BistHubContext _db;
        private readonly IHalkYatirimService _halkYatirimService;

        public StocksController(BistHubContext db, IHalkYatirimService halkYatirimService)
        {
            _db = db;
            _halkYatirimService = halkYatirimService;
        }

        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Returns all stocks")]
        public async Task<BaseListResponse<StockDto>> GetStocks(CancellationToken cancellationToken)
        {
            var stocks = await _db.Stocks.ToListAsync(cancellationToken);
            return BaseListResponse<StockDto>.Successful(stocks.Select(x => x.ToStockDto()));
        }

        [HttpGet("{stockCode}")]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Returns the stock by id")]
        public async Task<BaseResponse<StockDto>> GetStockById([FromRoute] string stockCode, CancellationToken cancellationToken)
        {
            var stock = await _db.Stocks.FindAsync(new[] { stockCode }, cancellationToken);
            if (stock == null)
                throw new BistHubException(404, Errors.StockNotFound, "Hisse senedi bulunamadı.");
            return BaseResponse<StockDto>.Successful(stock.ToStockDto());
        }

        [HttpGet("{stockCode}/periods")]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Returns the periods of stock")]
        public async Task<BaseListResponse<string>> GetStockPeriods([FromRoute] string stockCode, CancellationToken cancellationToken)
        {
            var periods = await _db.StockFinancials
                .Where(x => x.StockCode == stockCode)
                .GroupBy(x => x.Period)
                .Select(x => x.Key)
                .ToListAsync(cancellationToken);
            return BaseListResponse<string>.Successful(periods);
        }

        [HttpGet("{stockCode}/financials")]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Returns the financials of stock")]
        public async Task<BaseListResponse<StockFinancialDto>> GetStockFinancials([FromRoute] string stockCode, CancellationToken cancellationToken)
        {
            var financials = await _db.StockFinancials
                .Where(x => x.StockCode == stockCode)
                .OrderByDescending(x => x.Period)
                .ToListAsync(cancellationToken);
            if (financials.Any())
                return BaseListResponse<StockFinancialDto>.Successful(financials.Select(x => x.ToStockFinancialDto()));
            var table = await _halkYatirimService.GetStockFinancials(stockCode);
            var items = table.Values.Keys.Select(x => {
                var values = table.Values[x];
                var financial = new StockFinancial
                {
                    StockCode = stockCode,
                    Created = DateTime.Now,
                    Period = x,
                    NetSalesQuarterly = values["NetSatislarceyrek"],
                    NetSalesAnnualized = values["NetSatislarYillik"],
                    EbitdaQuarterly = values["FAVOKceyrek"],
                    EbitdaAnnualized = values["FAVOKYillik"],
                    NetFinancialIncomesQuarterly = values["FinansmanGiderlericeyrek"],
                    NetFinancialIncomesAnnualized = values["FinansmanGiderleriYillik"],
                    NetProfitQuarterly = values["NetKarceyrek"],
                    NetProfitAnnualized = values["NetKarDortDonem"],
                    TotalDebts = values["toplamborclar"],
                    NetDebts = values["NetBorc"],
                    NetWorkingCapital = values["NetisletmeSermayesi"],
                    Equity = values["Ozkaynaklar"],
                    Assets = values["ToplamAktifler"]
                };

                return financial;
            });
            _db.StockFinancials.AddRange(items);
            _db.SaveChangesAsync();
            return BaseListResponse<StockFinancialDto>.Successful(items.Select(x => x.ToStockFinancialDto()));
        }

        [HttpGet("{stockCode}/profitabilities")]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Returns the profitabilities of stock")]
        public async Task<BaseListResponse<StockProfitabilityDto>> GetStockProfitabilities([FromRoute] string stockCode, CancellationToken cancellationToken)
        {
            var profitabilities = await _db.StockProfitabilities
                .Where(x => x.StockCode == stockCode)
                .OrderByDescending(x => x.Period)
                .ToListAsync(cancellationToken);
            if (profitabilities.Any())
                return BaseListResponse<StockProfitabilityDto>.Successful(profitabilities.Select(x => x.ToStockProfitabilityDto()));
            var table = await _halkYatirimService.GetStockProfitabilities(stockCode);
            var items = table.Values.Keys.Select(x => {
                var values = table.Values[x];
                var financial = new StockProfitability
                {
                    StockCode = stockCode,
                    Created = DateTime.Now,
                    Period = x,
                    EbitdaMarginAnnualized = values["favokmarjiyillik"],
                    EbitdaMarginQuarterly = values["FavokMarjiceyrek"],
                    GrossProfitMarginAnnualized = values["BrutEsasFaaliyetKarMarjiYillik"],
                    GrossProfitMarginQuarterly = values["BrutEsasFaaliyetKariMarjiceyrek"],
                    NetDebtsToEbitda = values["orannetborcfavokceyrek"],
                    NetDebtsToEquity = values["ToplamBorcOzsermaye"],
                    NetFinancialIncomesToEbitda = values["netfinansmangelirgiderfavokceyrek"],
                    NetProfitMarginAnnualized = values["NetKarMarjiYillik"],
                    NetProfitMarginQuarterly = values["NetKarMarjiceyrek"],
                    OperatingExpensesMarginAnnualized = values["oranfaaliyetgiderlerifavokyillik"],
                    OperatingExpensesMarginQuarterly = values["oranfaaliyetgiderlerifavok"],
                    Roa = values["AktifKarlilik"],
                    Roe = values["OzsermayeKarliligiYillik"]
                };

                return financial;
            });
            _db.StockProfitabilities.AddRange(items);
            _db.SaveChangesAsync();
            return BaseListResponse<StockProfitabilityDto>.Successful(items.Select(x => x.ToStockProfitabilityDto()));
        }

        [HttpGet("favorites"), Authorize]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Returns all favorites stocks of the user")]
        public async Task<BaseListResponse<StockDto>> GetFavoriteStocks(CancellationToken cancellationToken)
        {
            var favorites = await _db.Favorites
                .Where(x => x.Username == HttpContext.GetUsername())
                .OrderBy(x => x.OrderNo)
                .Select(x => x.StockCodeNavigation)
                .ToListAsync(cancellationToken);
            return BaseListResponse<StockDto>.Successful(favorites.Select(x => x.ToStockDto()));
        }

        [HttpPut("favorites"), Authorize]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Updates user's favorite stocks")]
        public async Task<BaseResponse<int>> UpdateFavoriteStocks([FromBody] string[] stocks, CancellationToken cancellationToken)
        {
            _db.Favorites.RemoveRange(_db.Favorites.Where(x => x.Username == HttpContext.GetUsername()));
            for (short i = 0; i < stocks.Length; i++)
            {
                _db.Favorites.Add(new Favorite
                {
                    Username = HttpContext.GetUsername(),
                    OrderNo = i,
                    StockCode = stocks[i]
                });
            }
            var count =  await _db.SaveChangesAsync(cancellationToken);
            return BaseResponse<int>.Successful(count);
        }
    }
}
