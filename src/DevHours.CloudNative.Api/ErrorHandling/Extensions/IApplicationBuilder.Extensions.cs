using Microsoft.AspNetCore.Builder;

namespace DevHours.CloudNative.Api.ErrorHandling.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ErrorHandlerMiddleware>();
    }
}