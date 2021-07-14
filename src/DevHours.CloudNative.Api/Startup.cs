using DevHours.CloudNative.Core;
using DevHours.CloudNative.Infra;
using DevHours.CloudNative.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

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
            services.AddCloudNativeCore()
                    .AddCloudNativeInfrastructure();
                    
            services.AddControllers(options => options.EnableEndpointRouting = false)
                    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null)
                    .AddOData(o =>
                    {
                        o.Count().Filter().Expand().Select().OrderBy().SetMaxTop(25);
                        o.AddRouteComponents(GetEdmModel());
                    });
        }

        private IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();//.EnableLowerCamelCase();

            var rooms = builder.EntitySet<Room>(nameof(Room));
            rooms.EntityType.HasKey(r => r.Id);
            rooms.EntityType.HasMany(r => r.Bookings);

            var bookings = builder.EntitySet<Booking>(nameof(Booking));
            bookings.EntityType.HasKey(b => b.Id);

            return builder.GetEdmModel();
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
