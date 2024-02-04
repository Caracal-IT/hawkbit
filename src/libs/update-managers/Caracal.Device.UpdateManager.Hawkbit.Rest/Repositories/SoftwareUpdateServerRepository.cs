using System.Runtime.CompilerServices;
using Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.UpdateRequestsModels;
using Caracal.Device.UpdateManager.Models.UpdateRequest;
using Caracal.Device.UpdateManager.Repositories;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.Repositories;

public sealed class SoftwareUpdateServerRepository : ISoftwareUpdateServerRepository
{
    public async IAsyncEnumerable<ISoftwareUpdateRequest> GetSoftwareUpdateRequestsAsync(string tenantId, string deviceId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (true)
        {
            await Task.Delay(3000, cancellationToken);

            yield return new DeploymentRequest
            {
                Id = 4,
                Deployment = new Deployment
                {
                    Chunks = new List<Chunk>(),
                    DownloadType = "Type 1",
                    UpdateType = "ee"
                }
            };
        }
    }
}