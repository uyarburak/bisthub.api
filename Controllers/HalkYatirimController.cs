using BistHub.Api.Common;
using BistHub.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BistHub.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HalkYatirimController : ControllerBase
    {
        private readonly IHalkYatirimService _halkYatirimService;
        public HalkYatirimController(IHalkYatirimService halkYatirimService)
        {
            _halkYatirimService = halkYatirimService;
        }

        [HttpGet("stocks/{stockName}/periods")]
        public async Task<BaseListResponse<string>> GetPeriods([FromRoute] string stockName, CancellationToken cancellationToken)
        {
            return BaseListResponse<string>.Successful(await _halkYatirimService.GetPeriods(stockName, cancellationToken));
        }

        [HttpGet("stocks/{stockName}/periods/{periodYear}/{periodMonth}/ratios")]
        public async Task<BaseResponse<Dictionary<string, object>>> GetRatios([FromRoute] string stockName, [FromRoute] string periodYear, [FromRoute] string periodMonth, CancellationToken cancellationToken)
        {
            return BaseResponse<Dictionary<string, object>>.Successful(
                await _halkYatirimService.GetStockRatios(stockName, $"{periodYear}/{periodMonth}", cancellationToken)
            );
        }

        [HttpGet("stocks/{stockName}/profitabilities")]
        public async Task<BaseResponse<HalkYatirimTableDto>> GetProfitabilities([FromRoute] string stockName, CancellationToken cancellationToken)
        {
            return BaseResponse<HalkYatirimTableDto>.Successful(await _halkYatirimService.GetStockProfitabilities(stockName, cancellationToken));
        }

        [HttpGet("stocks/{stockName}/financials")]
        public async Task<BaseResponse<HalkYatirimTableDto>> GetFinancials([FromRoute] string stockName, CancellationToken cancellationToken)
        {
            return BaseResponse<HalkYatirimTableDto>.Successful(await _halkYatirimService.GetStockFinancials(stockName, cancellationToken));
        }
    }
}
