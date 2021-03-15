using Microsoft.AspNetCore.Http;

namespace BistHub.Api.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUsername(this HttpContext context)
        {
            return "burakuyar"; //TODO change after implement authentication/authorization
        }
    }
}
