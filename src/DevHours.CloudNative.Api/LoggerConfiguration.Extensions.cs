/*
 * Consumed and adapted from https://blog.johnnyreilly.com/2021/01/30/aspnet-serilog-and-application-insights/
 */

using System;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace DevHours.CloudNative.Api
{
    internal static class LoggerConfigurationExtensions
    {
        internal static void SetupLoggerConfiguration(string appName)
        {
            Log.Logger = new LoggerConfiguration()
                .ConfigureBaseLogging(appName)
                .CreateLogger();
        }

        internal static LoggerConfiguration ConfigureBaseLogging(
            this LoggerConfiguration loggerConfiguration,
            string appName
        )
        {
            loggerConfiguration
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.Async(a => a.Console(theme: AnsiConsoleTheme.Code))
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithProperty("ApplicationName", appName);

            return loggerConfiguration;
        }

        internal static LoggerConfiguration AddApplicationInsightsLogging(this LoggerConfiguration loggerConfiguration, IServiceProvider services, IConfiguration configuration)
        {
            if (!string.IsNullOrWhiteSpace(configuration.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY")))
            {
                loggerConfiguration.WriteTo.ApplicationInsights(
                    services.GetRequiredService<TelemetryConfiguration>(),
                    TelemetryConverter.Traces);
            }

            return loggerConfiguration;
        }
    }
}