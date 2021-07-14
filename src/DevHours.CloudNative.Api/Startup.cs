using System;
using DevHours.CloudNative.Api.ErrorHandling.Extensions;
using DevHours.CloudNative.Api.Data.OData.Extensions;
using DevHours.CloudNative.Core;
using DevHours.CloudNative.Infra;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
