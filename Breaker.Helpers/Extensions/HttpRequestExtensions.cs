using Microsoft.AspNetCore.Http;

namespace Breaker.Helpers.Extensions
{
    public static class HttpRequestExtentions
    {
        public static string GetFullHostingUrlRoot(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host.Value}";
        }
    }
}
