using BistHub.Api.Controllers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BistHub.Api.Services
{
    public interface IHalkYatirimService
    {
        Task<IEnumerable<string>> GetPeriods(string stockCode, CancellationToken cancellationToken = default);
        Task<HalkYatirimTableDto> GetStockFinancials(string stockCode, CancellationToken cancellationToken = default);
        Task<HalkYatirimTableDto> GetStockProfitabilities(string stockCode, CancellationToken cancellationToken = default);
        Task<Dictionary<string, object>> GetStockRatios(string stockCode, string period, CancellationToken cancellationToken = default);
    }

    public class HalkYatirimTableDto
    {
        public string Title { get; set; }
        public IEnumerable<HalkYatirimColumnDto> Columns { get; set; }
        public Dictionary<string, Dictionary<string, decimal?>> Values { get; set; }
    }

    public class HalkYatirimColumnDto
    {
        public string Title { get; set; }
        public string Property { get; set; }
        public string Format { get; set; }
    }
}
