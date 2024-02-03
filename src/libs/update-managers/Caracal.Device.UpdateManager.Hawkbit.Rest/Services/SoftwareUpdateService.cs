using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.Services;

public class SoftwareUpdateService(ILogger<SoftwareUpdateService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Software Update Service is running");

            await Task.Delay(3000, stoppingToken).ConfigureAwait(false);
        }
    }
}