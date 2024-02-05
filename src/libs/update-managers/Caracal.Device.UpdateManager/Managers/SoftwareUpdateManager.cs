using Caracal.Device.UpdateManager.Models.UpdateRequestsModels;
using Caracal.Device.UpdateManager.Repositories;
using Microsoft.Extensions.Logging;

namespace Caracal.Device.UpdateManager.Managers;

public sealed class SoftwareUpdateManager(ILogger<SoftwareUpdateManager> logger, ISoftwareUpdateServerRepository repository)
{
    public async Task CheckForUpdatesAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default)
    {
        await foreach (var request in repository.GetDeploymentsAsync(tenantId, deviceId, cancellationToken))
        {
            if(logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Checking for updates {RequestId}", request.Id); 
            
            await DownloadChunksAsync(request.Deployment, cancellationToken);
        }
    }

    private async Task DownloadChunksAsync(Deployment requestDeployment, CancellationToken cancellationToken)
    {
        if (!requestDeployment.Chunks.Any())
            return;
        
        foreach (var chunk in requestDeployment.Chunks)
            await  DownloadChunkAsync(chunk, cancellationToken);

    }

    private async Task DownloadChunkAsync(Chunk chunk, CancellationToken cancellationToken)
    {
        foreach (var artifact in chunk.Artifacts)
        {
           await DownloadArtifactsAsync(artifact, cancellationToken);
        }
    }

    private async Task DownloadArtifactsAsync(Artifact artifact, CancellationToken cancellationToken)
    {
        var data = await repository.DownloadArtifactAsync(artifact, cancellationToken).ConfigureAwait(false);
    }
}