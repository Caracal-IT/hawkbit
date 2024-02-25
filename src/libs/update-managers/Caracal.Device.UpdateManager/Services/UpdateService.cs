using Microsoft.Extensions.Hosting;
using Serilog;

namespace Caracal.Device.UpdateManager.Services;

public sealed class UpdateService(ILogger logger): BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.Information("{Output}", "Running UpdateService");
            await Task.Delay(120_000, stoppingToken);
        }
    }
}