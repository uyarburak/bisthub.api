using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BistHub.Api.Middlewares
{
    public class TraceLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TraceLoggerMiddleware> _logger;

        public TraceLoggerMiddleware(RequestDelegate next, ILogger<TraceLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;
            if (!request.Path.StartsWithSegments(new PathString("/api")))
            {
                await _next(httpContext);
                return;
            }
            var stopWatch = Stopwatch.StartNew();
            var requestTime = DateTime.UtcNow;

            var requestBodyContent = await ReadRequestBody(request);
            var originalBodyStream = httpContext.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                var response = httpContext.Response;
                response.Body = responseBody;
                await _next(httpContext);
                stopWatch.Stop();

                string responseBodyContent = null;
                responseBodyContent = await ReadResponseBody(response);
                await responseBody.CopyToAsync(originalBodyStream);

                SafeLog(requestTime,
                    stopWatch.ElapsedMilliseconds,
                    response.StatusCode,
                    request.Method,
                    request.Path,
                    request.QueryString.ToString(),
                    requestBodyContent,
                    responseBodyContent,
                    httpContext.Connection.RemoteIpAddress.MapToIPv4().ToString());
            }
        }

        private void SafeLog(DateTime requestTime,
                    long responseMillis,
                    int statusCode,
                    string method,
                    string path,
                    string queryString,
                    string requestBody,
                    string responseBody,
                    string ipAddress)
        {
            if (path.ToLower().StartsWith("/api/v1/auth"))
            {
                requestBody = "(Request logging disabled for /api/auth)";
                responseBody = "(Response logging disabled for /api/auth)";
            }

            if (requestBody.Length > 100)
            {
                requestBody = $"(Truncated to 100 chars) {requestBody.Substring(0, 100)}";
            }

            if (responseBody.Length > 100)
            {
                responseBody = $"(Truncated to 100 chars) {responseBody.Substring(0, 100)}";
            }

            if (queryString.Length > 100)
            {
                queryString = $"(Truncated to 100 chars) {queryString.Substring(0, 100)}";
            }

            //Serilog.Log.Information($"Request: [{method} {path} {queryString} Body: {requestBody}], Response: [{statusCode} {responseBody}], Ip: [{ipAddress}], Time: {responseMillis}ms");
            Serilog.Log.Information("Request: [{Method} {Path} {QueryString} Body: {RequestBody}], Response: [{StatusCode} {ResponseBody}], Ip: [{IpAddress}], Time: {ResponseMillis}ms",
                method, path, queryString, requestBody, statusCode, responseBody, ipAddress, responseMillis);
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }
    }
}
