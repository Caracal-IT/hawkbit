﻿using Caracal.Device.UpdateManager.Models.UpdateRequestsModels;
using Caracal.Device.UpdateManager.Repositories;
using Serilog;
using Serilog.Events;
// ReSharper disable ClassNeverInstantiated.Global

namespace Caracal.Device.UpdateManager.Managers;

public sealed class SoftwareUpdateManager(ILogger logger, ISoftwareUpdateServerRepository repository)
{
    public async Task CheckForUpdatesAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default)
    {
        await foreach (var request in repository.GetDeploymentsAsync(tenantId, deviceId, cancellationToken))
        {
            if(logger.IsEnabled(LogEventLevel.Information))
                logger.Information("Checking for updates {RequestId}", request.Id); 
            
            await DownloadChunksAsync(request.Deployment, cancellationToken);

            await repository.UpdateStatusAsync(tenantId, deviceId, request.Id, cancellationToken);
        }
    }

    private async Task DownloadChunksAsync(Deployment requestDeployment, CancellationToken cancellationToken)
    {
        if (requestDeployment.Chunks.Count == 0)
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
        
        var folder = $@"C:\SVC\software-update\{DateTime.UtcNow:dd-MMM-yyyy HH-mm-ss-fff}";

        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        
        var path = $@"{folder}\{artifact.Filename}";
        await File.WriteAllBytesAsync(path, data, cancellationToken).ConfigureAwait(false);
    }
}