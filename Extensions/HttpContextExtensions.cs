using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BistHub.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUsername(this HttpContext context)
        {
            return context.User.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
