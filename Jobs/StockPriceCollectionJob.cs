using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BistHub.Api.Jobs
{
    public class StockPriceCollectionJob : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public StockPriceCollectionJob(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Serilog.Log.Debug("StockPriceCollectionJob is starting.");
            stoppingToken.Register(() => Serilog.Log.Debug("StockPriceCollectionJob task is STOPPING"));

            while(!stoppingToken.IsCancellationRequested)
            {
                Serilog.Log.Debug("StockPriceCollectionJob is working.");
                CollectPrice(stoppingToken);
                var delayForNext1Am = DateTime.Today.AddDays(1).AddHours(1) - DateTime.Now;
                Serilog.Log.Information("Next iteration will begin in {Delay}", delayForNext1Am);
                await Task.Delay(delayForNext1Am, stoppingToken);
            }
            Serilog.Log.Debug("StockPriceCollectionJob task is STOPPING");
        }

        private async void CollectPrice(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                using var db = scope.ServiceProvider.GetRequiredService<Data.BistHubContext>();
                var yesterday = DateTime.Today.Subtract(TimeSpan.FromDays(1));
                var stocksToCollect = db.StockPriceViews
                    .Where(x => x.Max == null || x.Max < yesterday)
                    .OrderBy(x => x.Code)
                    .ToList();
                foreach (var stock in stocksToCollect)
                {
                    var startDate = stock.Max.HasValue ? stock.Max.Value.AddDays(1) : new DateTime(2015, 1, 1);
                    var endDate = yesterday;
                    try
                    {
                        Serilog.Log.Information($"{stock.Code} {startDate:dd.MM.yyyy} => {endDate:dd.MM.yyyy}");
                        await CollectPrice(db, stock.Code, startDate, endDate, stoppingToken);
                    }
                    catch (Exception e)
                    {
                        Serilog.Log.Error(e, "Error while getting {StockCode} stocks price data between {StartDate} and {EndDate}", stock.Code, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                    }
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Error in CollectPrice method");
            }

            Serilog.Log.Information($"CollectPrice iteration is over.");
        }
        private static Task<string> Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.Timeout = 10000;

            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEndAsync();
        }
        private async Task CollectPrice(Data.BistHubContext db, string code, DateTime startDate, DateTime endDate, CancellationToken stoppingToken)
        {
            var url = $"https://web-paragaranti-pubsub.foreks.com/web-services/historical-data?userName=undefined&name={code}&exchange=BIST&market=N&group=E&last=300&period=1440&intraPeriod=null&isLast=false&from={startDate:yyyyMMdd}000000&to={endDate:yyyyMMdd}235900";
            var json = await Get(url);
            var response = JsonConvert.DeserializeObject<ParaGarantiResponse>(json);

            db.StockPrices.AddRange(response.DataSet.Select(x => new StockPrice
            {
                StockCode = code,
                Date = x.DateDt,
                Close = x.Close,
                Open = x.Open,
                Low = x.Low,
                High = x.High
            }));
            await db.SaveChangesAsync(stoppingToken);
        }
    }

    class ParaGarantiPriceDto
    {
        public decimal Close { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Open { get; set; }
        public long Date { get; set; }
        public DateTime DateDt => DateTimeOffset.FromUnixTimeMilliseconds(Date).LocalDateTime;
    }

    class ParaGarantiResponse
    {
        public List<ParaGarantiPriceDto> DataSet { get; set; }
    }
}
