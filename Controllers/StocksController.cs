using BistHub.Api.Common;
using BistHub.Api.Data;
using BistHub.Api.Dtos;
using BistHub.Api.Exceptions;
using BistHub.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BistHub.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly BistHubContext _db;

        public StocksController(BistHubContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<BaseListResponse<StockDto>> GetStocks(CancellationToken cancellationToken)
        {
            var stocks = await _db.Stocks.ToListAsync(cancellationToken);
            return BaseListResponse<StockDto>.Successful(stocks.Select(x => x.ToStockDto()));
        }

        [HttpGet("{stockCode}")]
        public async Task<BaseResponse<StockDto>> GetStockById([FromRoute] string stockCode, CancellationToken cancellationToken)
        {
            var stock = await _db.Stocks.FindAsync(new[] { stockCode }, cancellationToken);
            if (stock == null)
                throw new BistHubException(404, Errors.StockNotFound, "Hisse senedi bulunamadı.");
            return BaseResponse<StockDto>.Successful(stock.ToStockDto());
        }

        [HttpGet("favorites")]
        public async Task<BaseListResponse<StockDto>> GetFavoriteStocks(CancellationToken cancellationToken)
        {
            var favorites = await _db.Favorites
                .Where(x => x.Username == HttpContext.GetUsername())
                .OrderBy(x => x.OrderNo)
                .Select(x => x.StockCodeNavigation)
                .ToListAsync(cancellationToken);
            return BaseListResponse<StockDto>.Successful(favorites.Select(x => x.ToStockDto()));
        }

        [HttpPut("favorites")]
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
