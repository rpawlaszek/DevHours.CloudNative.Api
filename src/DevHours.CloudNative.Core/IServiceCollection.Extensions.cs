using DevHours.CloudNative.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DevHours.CloudNative.Core
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudNativeCore(this IServiceCollection services)
        {
            services.AddScoped<RoomImagesService>();
            return services;
        }
    }
}