using Microsoft.Extensions.DependencyInjection;

namespace DevHours.CloudNative.Api.Exceptions.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddErrorHandler(this IServiceCollection services)
            => services.AddTransient<ErrorHandlerMiddleware>();
    }
}