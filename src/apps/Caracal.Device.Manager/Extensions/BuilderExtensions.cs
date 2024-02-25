using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace Caracal.Device.Manager.Extensions;

public static class BuilderExtensions
{
    public static HostApplicationBuilder AddBaseServices(this HostApplicationBuilder builder)
    {
        builder.Configuration
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables();
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        
        builder.Services
               .AddSingleton(Log.Logger)
               .AddLogging(b => {
                    b.ClearProviders();
                    b.AddSerilog(dispose: true);
               });

        builder.Services.AddHttpClient(HttpClients.HawkHttpClient, x =>
        {
            x.BaseAddress = new Uri("http://localhost:8080");
            x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("GatewayToken", "2677aea31a570f07d594d3439c1299f2");
        });
        
        return builder;
    }
}