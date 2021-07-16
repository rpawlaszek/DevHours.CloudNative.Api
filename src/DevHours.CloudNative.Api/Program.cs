using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace DevHours.CloudNative.Api
{
    public class Program
    {
        const string APP_NAME = "DH.Backend";

        public static void Main(string[] args)
        {
            LoggerConfigurationExtensions.SetupLoggerConfiguration(APP_NAME);

            try
            {
                Log.Information("Starting Web Host");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostBuilderContext, services, loggerConfiguration) =>
                {
                    loggerConfiguration.ConfigureBaseLogging(APP_NAME);
                    loggerConfiguration.AddApplicationInsightsLogging(services, hostBuilderContext.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
