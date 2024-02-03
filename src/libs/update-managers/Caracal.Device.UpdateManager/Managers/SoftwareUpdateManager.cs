using Caracal.Device.UpdateManager.Repositories;
using Microsoft.Extensions.Logging;

namespace Caracal.Device.UpdateManager.Managers;

public sealed class SoftwareUpdateManager(ILogger<SoftwareUpdateManager> logger, ISoftwareUpdateServerRepository repository)
{
    public Task CheckForUpdatesAsync(CancellationToken cancellationToken = default)
    {
        if(logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Checking for updates {UpdateServerUrl}", repository.GetSoftwareUpdateServerUrlAsync(cancellationToken).Result);
        
        return Task.CompletedTask;
    }
}