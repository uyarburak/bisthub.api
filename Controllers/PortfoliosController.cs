using BistHub.Api.Common;
using BistHub.Api.Data;
using BistHub.Api.Dtos;
using BistHub.Api.Exceptions;
using BistHub.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BistHub.Api.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BistHub.Api.Controllers
{
    [ApiController, Authorize]
    [Route("api/v1/[controller]")]
    public class PortfoliosController : ControllerBase
    {
        private readonly BistHubContext _db;

        public PortfoliosController(BistHubContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Returns all portfolios of user")]
        public async Task<BaseListResponse<PortfolioDto>> GetPortfolios(CancellationToken cancellationToken)
        {
            var portfolios = await _db.Portfolios
                .Where(x => x.Username == HttpContext.GetUsername())
                .ToListAsync(cancellationToken);
            return BaseListResponse<PortfolioDto>.Successful(portfolios.Select(x => x.ToPortfolioDto()));
        }

        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Creates new portfolio")]
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
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Updates portfolio's title")]
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
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Returns positions of portfolio")]
        public async Task<BaseListResponse<PositionDto>> GetPositions([FromRoute] Guid portfolioId, CancellationToken cancellationToken)
        {
            var portfolio = await TryFindPortfolio(portfolioId, cancellationToken);

            var positions = await _db.Positions
                .Where(x => x.PortfolioId == portfolioId)
                .ToListAsync(cancellationToken);
            return BaseListResponse<PositionDto>.Successful(positions.Select(x => x.ToPositionDto()));
        }

        [HttpPost("{portfolioId}/positions")]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Create an open position for portfolio")]
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
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Removes the position from portfolio")]
        public async Task<BaseResponse> DeletePosition([FromRoute] Guid portfolioId, [FromRoute] int positionId, CancellationToken cancellationToken)
        {
            var position = await _db.Positions
                .Include(x => x.Portfolio)
                .FirstOrDefaultAsync(x => x.Id == positionId, cancellationToken);
            //await _db.Positions.FindAsync(new object[] { positionId }, cancellationToken);
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
            throw new BistHubException(500, Errors.PositionNotRemoved, "Pozisyon silinirken hata meydana geldi");
        }

        [HttpPut("{portfolioId}/positions/{positionId}")]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Updates the position of portfolio")]
        public async Task<BaseResponse<PositionDto>> UpdatePosition([FromRoute] Guid portfolioId, [FromRoute] int positionId, [FromBody] UpdatePositionRequest request, CancellationToken cancellationToken)
        {
            var position = await _db.Positions
                .Include(x => x.Portfolio)
                .FirstOrDefaultAsync(x => x.Id == positionId, cancellationToken);
            //await _db.Positions.FindAsync(new object[] { positionId }, cancellationToken);
            if (position == null || position.PortfolioId != portfolioId)
                throw new BistHubException(404, Errors.PositionNotFound, "Pozisyon bulunamadı");
            var portfolio = position.Portfolio;
            //var portfolio = await _db.Portfolios.FindAsync(new[] { portfolioId }, cancellationToken);
            if (portfolio == null)
                throw new BistHubException(404, Errors.PortfolioNotFound, "Portföy bulunamadı");
            if (portfolio.Username != HttpContext.GetUsername())
                throw new BistHubException(403, Errors.NotOwnThePortfolio, "Portföye erişim yetkiniz yok");

            position.BuyDate = request.BuyDate;
            position.BuyPrice = request.BuyPrice;
            position.Amount = request.Amount;
            position.PaidFee = request.PaidFee;
            position.SellDate = request.SellDate;
            position.SellPrice = request.SellPrice;
            position.Updated = DateTime.Now;
            _db.Positions.Update(position);

            var updated = (await _db.SaveChangesAsync(cancellationToken)) == 1;
            if (updated)
                return BaseResponse<PositionDto>.Successful(position.ToPositionDto());
            throw new BistHubException(500, Errors.PositionNotRemoved, "Pozisyon güncellenirken hata meydana geldi");
        }

        [HttpGet("{portfolioId}/positions/open")]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Returns only open positions of the portfolio")]
        public async Task<BaseListResponse<PositionDto>> GetOpenPositions([FromRoute] Guid portfolioId, CancellationToken cancellationToken)
        {
            var portfolio = await TryFindPortfolio(portfolioId, cancellationToken);

            var positions = await _db.Positions
                .Where(x => x.PortfolioId == portfolioId && x.SellPrice == null)
                .ToListAsync(cancellationToken);
            return BaseListResponse<PositionDto>.Successful(positions.Select(x => x.ToPositionDto()));
        }

        [HttpGet("{portfolioId}/positions/close")]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation("Returns only close positions of the portfolio")]
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
