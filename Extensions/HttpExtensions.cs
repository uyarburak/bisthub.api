using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BistHub.Api.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<R> PostAsJsonAsync<T, R>(
            this HttpClient httpClient, string url, T data)
        {
            var response = await httpClient.PostAsJsonAsync<T>(url, data);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsJsonAsync<R>();
            }
            else
            {
                return default(R);
            }


        }
        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(
            this HttpClient httpClient, string url, T data)
        {
            var options = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
                Formatting = Formatting.Indented
            };

            var dataAsString = JsonConvert.SerializeObject(data, options);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return httpClient.PostAsync(url, content);
        }

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(dataAsString);
        }
    }
}
