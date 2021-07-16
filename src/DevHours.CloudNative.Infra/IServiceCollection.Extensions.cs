using DevHours.CloudNative.DataAccess;
using DevHours.CloudNative.Domain;
using DevHours.CloudNative.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevHours.CloudNative.Infra
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCloudNativeInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HotelContext>(o =>
            {
                var hotelConnectionString = configuration.GetConnectionString("HotelContext");

                if (string.IsNullOrWhiteSpace(hotelConnectionString))
                {
                    o.UseInMemoryDatabase("hoteldb")
                     .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }
                else
                {
                    o.UseSqlServer(connectionString: hotelConnectionString)
                     .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }
            });

            services.AddScoped<IDataRepository<Room>, RoomsRepository>();
            services.AddScoped<IDataRepository<Booking>, BookingRepository>();

            services.AddScoped<IBlobRepository<string>>(provider =>
            {
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