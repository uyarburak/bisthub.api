using BistHub.Api.Common;
using BistHub.Api.Data;
using BistHub.Api.Dtos;
using BistHub.Api.Exceptions;
using BistHub.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BistHub.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PortfoliosController : ControllerBase
    {
        private readonly BistHubContext _db;

        public PortfoliosController(BistHubContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<BaseListResponse<PortfolioDto>> GetPortfolios(CancellationToken cancellationToken)
        {
            var portfolios = await _db.Portfolios
                .Where(x => x.Username == HttpContext.GetUsername())
                .ToListAsync(cancellationToken);
            return BaseListResponse<PortfolioDto>.Successful(portfolios.Select(x => x.ToPortfolioDto()));
        }

        [HttpPost]
        public async Task<BaseResponse<PortfolioDto>> CreatePortfolio([FromBody] CreatePortfolioRequest request, CancellationToken cancellationToken)
        {
            var portfolio = new Portfolio
            {
                Title = request.Title,
                Username = HttpContext.GetUsername()
            };
            _db.Portfolios.Add(portfolio);

            var inserted = (await _db.SaveChangesAsync(cancellationToken)) == 1;
            if (inserted)
                return BaseResponse<PortfolioDto>.Successful(portfolio.ToPortfolioDto());
            throw new BistHubException(500, Errors.PortfolioNotCreated, "Portföy oluşturulurken hata meydana geldi");
        }

        [HttpPut("{portfolioId}/title")]
        public async Task<BaseResponse<PortfolioDto>> UpdatePortfolioTitle([FromRoute] Guid portfolioId, [FromBody] CreatePortfolioRequest request, CancellationToken cancellationToken)
        {
            var portfolio = await TryFindPortfolio(portfolioId, cancellationToken);

            portfolio.Title = request.Title;
            var updated = (await _db.SaveChangesAsync(cancellationToken)) == 1;
            if (updated)
                return BaseResponse<PortfolioDto>.Successful(portfolio.ToPortfolioDto());
            throw new BistHubException(500, Errors.PortfolioTitleNotUpdated, "Portföy başlığı değiştirilirken hata meydana geldi");
        }

        [HttpGet("{portfolioId}/positions")]
        public async Task<BaseListResponse<PositionDto>> GetPositions([FromRoute] Guid portfolioId, CancellationToken cancellationToken)
        {
            var portfolio = await TryFindPortfolio(portfolioId, cancellationToken);

            var positions = await _db.Positions
                .Where(x => x.PortfolioId == portfolioId)
                .ToListAsync(cancellationToken);
            return BaseListResponse<PositionDto>.Successful(positions.Select(x => x.ToPositionDto()));
        }

        [HttpPost("{portfolioId}/positions")]
        public async Task<BaseResponse<PositionDto>> CreatePosition([FromRoute] Guid portfolioId, [FromBody] CreatePositionRequest request, CancellationToken cancellationToken)
        {
            var portfolio = await TryFindPortfolio(portfolioId, cancellationToken);

            var position = new Position
            {
                PortfolioId = portfolioId,
                StockCode = request.StockCode,
                Amount = request.Amount,
                PaidFee = request.PaidFee,
                BuyDate = request.BuyDate,
                BuyPrice = request.BuyPrice
            };
            _db.Positions.Add(position);

            var inserted = (await _db.SaveChangesAsync(cancellationToken)) == 1;
            if (inserted)
                return BaseResponse<PositionDto>.Successful(position.ToPositionDto());
            throw new BistHubException(500, Errors.PositionNotCreated, "Pozisyon oluşturulurken hata meydana geldi");
        }

        [HttpDelete("{portfolioId}/positions/{positionId}")]
        public async Task<BaseResponse> DeletePosition([FromRoute] Guid portfolioId, [FromRoute] int positionId, CancellationToken cancellationToken)
        {
            var position = await _db.Positions.FindAsync(new object[] { positionId }, cancellationToken);
            if (position == null || position.PortfolioId != portfolioId)
                throw new BistHubException(404, Errors.PositionNotFound, "Pozisyon bulunamadı");
            var portfolio = position.Portfolio;
            //var portfolio = await _db.Portfolios.FindAsync(new[] { portfolioId }, cancellationToken);
            if (portfolio == null)
                throw new BistHubException(404, Errors.PortfolioNotFound, "Portföy bulunamadı");
            if (portfolio.Username != HttpContext.GetUsername())
                throw new BistHubException(403, Errors.NotOwnThePortfolio, "Portföye erişim yetkiniz yok");

            _db.Positions.Remove(position);

            var removed = (await _db.SaveChangesAsync(cancellationToken)) == 1;
            if (removed)
                return BaseResponse.Successful();
            throw new Exceptions.BistHubException(500, Errors.PositionNotRemoved, "Pozisyon silinirken hata meydana geldi");
        }

        [HttpGet("{portfolioId}/positions/open")]
        public async Task<BaseListResponse<PositionDto>> GetOpenPositions([FromRoute] Guid portfolioId, CancellationToken cancellationToken)
        {
            var portfolio = await TryFindPortfolio(portfolioId, cancellationToken);

            var positions = await _db.Positions
                .Where(x => x.PortfolioId == portfolioId && x.SellPrice == null)
                .ToListAsync(cancellationToken);
            return BaseListResponse<PositionDto>.Successful(positions.Select(x => x.ToPositionDto()));
        }

        [HttpGet("{portfolioId}/positions/close")]
        public async Task<BaseListResponse<PositionDto>> GetClosePositions([FromRoute] Guid portfolioId, CancellationToken cancellationToken)
        {
            var portfolio = await TryFindPortfolio(portfolioId, cancellationToken);

            var positions = await _db.Positions
                .Where(x => x.PortfolioId == portfolioId && x.SellPrice != null)
                .ToListAsync(cancellationToken);
            return BaseListResponse<PositionDto>.Successful(positions.Select(x => x.ToPositionDto()));
        }

        private async ValueTask<Portfolio> TryFindPortfolio(Guid portfolioId, CancellationToken cancellationToken)
        {
            var portfolio = await _db.Portfolios.FindAsync(new object[] { portfolioId }, cancellationToken);
            if (portfolio == null)
                throw new BistHubException(404, Errors.PortfolioNotFound, "Portföy bulunamadı");
            if (portfolio.Username != HttpContext.GetUsername())
                throw new BistHubException(403, Errors.NotOwnThePortfolio, "Portföye erişim yetkiniz yok");
            return portfolio;
        }
    }
}
