using DevHours.CloudNative.Api.Data.OData.Extensions;
using DevHours.CloudNative.Api.ErrorHandling.Extensions;
using DevHours.CloudNative.Core;
using DevHours.CloudNative.DataAccess;
using DevHours.CloudNative.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

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
                    .AddCloudNativeInfrastructure(Configuration);

            services.AddControllers(options => options.EnableEndpointRouting = false)
                    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null)
                    .AddODataBindings();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddErrorHandler();
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

            app.UseErrorHandler();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<HotelContext>();
                dbContext.Database.EnsureCreated();
            }
        }
    }
}
