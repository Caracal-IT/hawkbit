using Caracal.Device.UpdateManager.Repositories;
using Microsoft.Extensions.Logging;

namespace Caracal.Device.UpdateManager.Managers;

public sealed class SoftwareUpdateManager(ILogger<SoftwareUpdateManager> logger, ISoftwareUpdateServerRepository repository)
{
    public async Task CheckForUpdatesAsync(CancellationToken cancellationToken = default)
    {
        await foreach (var request in repository.GetSoftwareUpdateRequestsAsync("", "", cancellationToken))
        {
            if(logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Checking for updates {RequestId}", request.Id);     
        }
    }
}