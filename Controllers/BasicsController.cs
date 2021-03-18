using BistHub.Api.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BistHub.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasicsController : ControllerBase
    {
        private readonly GoogleSheetsConfig _googleSheetsConfig;
        public BasicsController(IOptions<GoogleSheetsConfig> options)
        {
            _googleSheetsConfig = options.Value;
        }

        [HttpGet("custom")]
        public async Task<BaseListResponse<string>> GetCustomSheets(CancellationToken cancellationToken)
        {
            return BaseListResponse<string>.Successful(_googleSheetsConfig.CustomSheets.Keys);
        }

        [HttpGet("custom/{sheetName}")]
        public async Task<BaseListResponse<List<object>>> GetCustomSheet([FromRoute] string sheetName, CancellationToken cancellationToken)
        {
            if (!_googleSheetsConfig.CustomSheets.ContainsKey(sheetName))
                throw new Exceptions.BistHubException(404, "CUSTOM_SHEET_NOT_FOUND", "Dosya bulunamadı");
            return BaseListResponse<List<object>>.Successful(
                await GetSheet(_googleSheetsConfig.CustomSheets[sheetName], cancellationToken)
            );
        }

        [HttpGet("autoBalance")]
        public async Task<BaseListResponse<List<object>>> GetAutoBalanceSheet(CancellationToken cancellationToken)
        {
            return BaseListResponse<List<object>>.Successful(
                await GetSheet("'Oto Bilanço'!A1:AB400", cancellationToken)
            );
        }

        [HttpGet("balancePoint")]
        public async Task<BaseListResponse<List<object>>> GetBalancePointSheet(CancellationToken cancellationToken)
        {
            return BaseListResponse<List<object>>.Successful(
                await GetSheet("'Son Bilanço Temel Puan'!A1:J400", cancellationToken)
            );
        }

        [HttpGet("dividends")]
        public async Task<BaseListResponse<List<object>>> GetDividendSheet(CancellationToken cancellationToken)
        {
            return BaseListResponse<List<object>>.Successful(
                await GetSheet("'Temettü'!A1:S200", cancellationToken)
            );
        }

        [HttpGet("priceEarnings")]
        public async Task<BaseListResponse<List<object>>> GetPriceEarningRatioSheet(CancellationToken cancellationToken)
        {
            return BaseListResponse<List<object>>.Successful(
                await GetSheet("'FK Çalışması'!A1:L400", cancellationToken)
            );
        }

        [HttpGet("longTerm")]
        public async Task<BaseListResponse<List<object>>> GetLongTermSheet(CancellationToken cancellationToken)
        {
            return BaseListResponse<List<object>>.Successful(
                await GetSheet("'UzunVadeListesi'!A1:AV200", cancellationToken)
            );
        }

        [HttpGet("isYatirimRatioAnalysis")]
        public async Task<BaseListResponse<List<object>>> GetIsYatirimRatioAnalysisSheet(CancellationToken cancellationToken)
        {
            return BaseListResponse<List<object>>.Successful(
                await GetSheet("'İsYatırımOranAnaliziPuanlama'!A1:Q450", cancellationToken)
            );
        }

        [HttpGet("isYatirimDiscounts")]
        public async Task<BaseListResponse<List<object>>> GetIsYatirimDiscountsSheet(CancellationToken cancellationToken)
        {
            return BaseListResponse<List<object>>.Successful(
                await GetSheet("'İsYatırımİskonto'!A1:K100", cancellationToken)
            );
        }

        [HttpGet("historicalProfits")]
        public async Task<BaseListResponse<List<object>>> GetHistoricalProfitsSheet(CancellationToken cancellationToken)
        {
            return BaseListResponse<List<object>>.Successful(
                await GetSheet("'Geçmiş Getiriler'!A1:S400", cancellationToken)
            );
        }

        [HttpGet("stockSplitProbabilities")]
        public async Task<BaseListResponse<List<object>>> GetStockSplitProbabilitiesSheet(CancellationToken cancellationToken)
        {
            return BaseListResponse<List<object>>.Successful(
                await GetSheet("'Bedelsiz'!A1:D400", cancellationToken)
            );
        }

        private async Task<IEnumerable<List<object>>> GetSheet(string range, CancellationToken cancellationToken)
        {
            var encoded = HttpUtility.HtmlDecode(range);
            var uri = $"https://content-sheets.googleapis.com/v4/spreadsheets/{_googleSheetsConfig.SheetId}/values/{encoded}?valueRenderOption=UNFORMATTED_VALUE&key={_googleSheetsConfig.ApiKey}";
            var response = await Get(uri, cancellationToken);
            var result = JsonConvert.DeserializeObject<SpreadSheetResponse>(response);

            return result.Values
                .Where(a => a.Any(x => x is not string || (!string.IsNullOrWhiteSpace((string) x) && !x.Equals("#N/A ()") && !x.Equals("#DIV/0! ()"))))
                .Select(a => a.Select(x => x is string && (x.Equals("#N/A ()") || x.Equals("#DIV/0! ()")) ? null : x).ToList());
        }

        private static readonly HttpClient client = new HttpClient();
        private static Task<string> Get(string uri, CancellationToken cancellationToken)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            return client.GetStringAsync(uri, cancellationToken);
        }
    }

    internal class SpreadSheetResponse
    {
        public List<List<object>> Values { get; set; }
    }
}
