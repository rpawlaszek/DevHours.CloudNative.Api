using Microsoft.EntityFrameworkCore;
using DevHours.CloudNative.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.OData;
using DevHours.CloudNative.Api.Data.OData;

namespace DevHours.CloudNative.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HotelContext>(o =>
            {
                o.UseInMemoryDatabase("hoteldb")
                 .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddControllers(options => options.EnableEndpointRouting = false)
                    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null)
                    .AddOData(o =>
                    {
                        o.Count().Filter().Expand().Select().OrderBy().SetMaxTop(25);
                        var builder = new HotelModelBuilder();
                        o.AddRouteComponents(builder.GetEdmModel());
                    });

            services.AddScoped<BlobContainerClient>(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                var blobServiceClient = new BlobServiceClient(configuration.GetSection("Images").GetValue<string>("ConnectionString"));
                return blobServiceClient.GetBlobContainerClient(configuration.GetSection("Images").GetValue<string>("ContainerName"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
