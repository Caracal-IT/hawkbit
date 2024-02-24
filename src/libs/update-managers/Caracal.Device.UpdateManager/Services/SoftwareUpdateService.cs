using Caracal.Device.UpdateManager.Managers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Caracal.Device.UpdateManager.Services;

public sealed class SoftwareUpdateService(ILogger<SoftwareUpdateService> logger, SoftwareUpdateManager softwareUpdateManager) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Software Update Service is running");
            
            await softwareUpdateManager.CheckForUpdatesAsync("default", "gate1", stoppingToken).ConfigureAwait(false);

            await Task.Delay(120_000, stoppingToken).ConfigureAwait(false);
        }
    }
}