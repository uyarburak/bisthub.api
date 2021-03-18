using BistHub.Api.Common;
using BistHub.Api.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BistHub.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                return;
            }
            catch(BistHubException e)
            {
                // Getting source of exception for logging.
                var method = new StackTrace(e).GetFrame(0).GetMethod();
                var source = $"{method.DeclaringType.FullName}.{method.Name}";

                // Logging the exception with its source
                Serilog.Log.Error(e, "Message: {ErrorMessage}, InnerException: {Exception}, Source: {Source}", e.Message, e.InnerException, source);

                // Preparing the response with status code
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = e.HttpCode;
                    var response = BaseResponse.Fail(e.ErrorCode, e.Message);
                    await context.Response.WriteAsJsonAsync(response);
                }
            }
            catch(Exception exception)
            {
                // Getting source of exception for logging.
                var method = new StackTrace(exception).GetFrame(0).GetMethod();
                var source = $"{method.DeclaringType.FullName}.{method.Name}";

                // Logging the exception with its source
                Serilog.Log.Error(exception, "Exception: {Exception}, Source: {Source}", exception, source);

                // Preparing the response with status code 500
                if (!context.Response.HasStarted)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 500;
                    var response = BaseResponse.Fail("Unknown error");
                    await context.Response.WriteAsJsonAsync(response);
                }
            }
        }
    }
}
