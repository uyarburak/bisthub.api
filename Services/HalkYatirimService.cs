using BistHub.Api.Controllers;
using MoreLinq.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BistHub.Api.Services
{
    public class HalkYatirimService : IHalkYatirimService
    {
        private readonly HttpClient _client = new HttpClient();
        private const string ServiceUrl = "https://www.halkyatirim.com.tr/Servis/FinnetService?Metod=POST";

        public async Task<HalkYatirimTableDto> GetStockFinancials(string stockCode, CancellationToken cancellationToken = default)
        {
            var periods = await GetPeriods(stockCode, cancellationToken);
            HalkYatirimTableDto table = null;
            foreach(var batch in periods.Batch(16))
            {
                var requestJson = JsonConvert.SerializeObject(new Dictionary<string, string> {
                    { "Param", "{\"RaporParams\":{\"Url\":\"cms-halkyatirim-finansaltablolar-revize-finansallar-revizeson\",\"RaporParametreleri\":[{\"key\":\"Kod\",\"value\":\"" + stockCode + "\"},{\"key\":\"Donemler\",\"value\":\"" + string.Join(',', batch) + "\"}]}}" },
                    { "ServisUrl", "RaporTabloHesapla" }
                });
                var result = await GetResult<TemelAnaliz2>(requestJson, cancellationToken);
                var dto = ConvertToTableDto(result);
                if (table == null)
                    table = dto;
                else
                    dto.Values.Keys.ForEach(x => table.Values.Add(x, dto.Values[x]));
            }
            return table;
        }

        public async Task<HalkYatirimTableDto> GetStockProfitabilities(string stockCode, CancellationToken cancellationToken = default)
        {
            var periods = await GetPeriods(stockCode, cancellationToken);
            HalkYatirimTableDto table = null;
            foreach (var batch in periods.Batch(16))
            {
                var requestJson = JsonConvert.SerializeObject(new Dictionary<string, string> {
                    { "Param", "{\"RaporParams\":{\"Url\":\"cms-halkyatirim-finansaltablolar-revize-karlilik-revizeson\",\"RaporParametreleri\":[{\"key\":\"Kod\",\"value\":\"" + stockCode + "\"},{\"key\":\"Donemler\",\"value\":\"" + string.Join(',', batch) + "\"}]}}" },
                    { "ServisUrl", "RaporTabloHesapla" }
                });
                var result = await GetResult<TemelAnaliz2>(requestJson, cancellationToken);
                var dto = ConvertToTableDto(result);
                if (table == null)
                    table = dto;
                else
                    dto.Values.Keys.ForEach(x => table.Values.Add(x, dto.Values[x]));
            }
            return table;
        }

        public async Task<Dictionary<string, object>> GetStockRatios(string stockCode, string period, CancellationToken cancellationToken = default)
        {
            var requestJson = JsonConvert.SerializeObject(new Dictionary<string, string> {
                { "Param", "{\"RaporParams\":{\"Url\":\"cms-halkyatirim-sanayi-sirket-kart-temel-analiz-verileri\",\"RaporParametreleri\":[{\"key\":\"Kod\",\"value\":\"" + stockCode + "\"},{\"key\":\"HesapDonem\",\"value\":\"" + period + "\"},{\"key\":\"Tarih\",\"value\":\"" + DateTime.Today.ToString("yyyy-MM-dd") + "\"}]}}" },
                { "ServisUrl", "RaporTabloHesapla" }
            });
            var result = await GetResult<TemelAnaliz>(requestJson, cancellationToken);
            return result.Tablo.JSVeriler
                .Select(x => x.O)
                .ToDictionary(x => x.Baslik, x => x.Baslik1);
        }

        public async Task<IEnumerable<string>> GetPeriods(string stockCode, CancellationToken cancellationToken = default)
        {
            var requestJson = JsonConvert.SerializeObject(new Dictionary<string, string> {
                { "Param", "{\"Param\":{\"Kod\":\"" + stockCode + "\"}" },
                { "ServisUrl", "HisseDonemListesi" }
            });
            var result = await GetResult<HisseDonemListesi>(requestJson, cancellationToken);
            return result.HisseDonemListesiResult.Select(x => x.Id);
        }

        private static HalkYatirimTableDto ConvertToTableDto(TemelAnaliz2 data)
        {
            var table = new HalkYatirimTableDto();
            table.Title = data.Tablo.Baslik;
            table.Columns = data.Tablo.BaslikListe.Select(x => new HalkYatirimColumnDto
            {
                Title = x.Baslik,
                Property = x.PropertyName,
                Format = x.VeriFormat
            });
            table.Values = data.Tablo.JSVeriler.ToDictionary(x => x.Baslik.Text, x => x.O);

            return table;
        }

        private async Task<T> GetResult<T>(string requestBody, CancellationToken cancellationToken)
        {
            var body = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(ServiceUrl, body, cancellationToken);
            if (!response.IsSuccessStatusCode)
                throw new Exceptions.BistHubException(500, "", "");
            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            json = json.Replace("\\\"", "\"").Replace("\\\\/", "/").Trim('"');
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    class HisseDonemListesi
    {
        public IEnumerable<HisseDonem> HisseDonemListesiResult { get; set; }
    }

    class HisseDonem
    {
        public string Id { get; set; }
    }

    class TemelAnaliz
    {
        public List<Tablo> TabloListesi { get; set; }
        public Tablo Tablo => TabloListesi[0];
    }

    class TemelAnaliz2
    {
        public List<Tablo2> TabloListesi { get; set; }
        public Tablo2 Tablo => TabloListesi[0];
    }

    class Tablo2
    {
        public IEnumerable<JSVeri2> JSVeriler { get; set; }
        public string Baslik { get; set; }
        public IEnumerable<BaslikListe> BaslikListe { get; set; }
    }

    class Tablo
    {
        public IEnumerable<JSVeri> JSVeriler { get; set; }
    }

    class BaslikListe
    {
        public string Baslik { get; set; }
        public string PropertyName { get; set; }
        public string VeriFormat { get; set; }
    }


    class JSVeri2
    {
        public Dictionary<string, decimal?> O { get; set; }
        public Baslik Baslik { get; set; }
    }

    class JSVeri
    {
        public KeyValue O { get; set; }
    }

    class Baslik
    {
        public string Text { get; set; }
    }

    class KeyValue
    {
        public string Baslik { get; set; }
        public object Baslik1 { get; set; }
    }
}
