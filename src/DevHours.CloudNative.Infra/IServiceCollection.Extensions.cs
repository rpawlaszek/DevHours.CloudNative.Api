using DevHours.CloudNative.Domain;
using DevHours.CloudNative.DataAccess;
using DevHours.CloudNative.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace DevHours.CloudNative.Infra
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudNativeInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<HotelContext>(o =>
                o.UseInMemoryDatabase("hotel")
                 .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );

            services.AddScoped<IDataRepository<Room>, RoomsRepository>();
            services.AddScoped<IDataRepository<Booking>, BookingRepository>();

            services.AddScoped<IBlobRepository<string>>(provider => {
                var configuration = provider.GetService<IConfiguration>();
                return new RoomImagesRepository(
                    configuration.GetSection("Images").GetValue<string>("ConnectionString"),
                    configuration.GetSection("Images").GetValue<string>("ContainerName")
                );
            });

            return services;
        }
    }
}