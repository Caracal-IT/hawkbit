using Caracal.Device.UpdateManager.Models.UpdateRequestsModels;

namespace Caracal.Device.UpdateManager.Repositories;

public interface ISoftwareUpdateServerRepository
{
    IAsyncEnumerable<DeploymentRequest> GetDeploymentsAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
    Task<byte[]> DownloadArtifactAsync(Artifact artifact, CancellationToken cancellationToken = default); 
}