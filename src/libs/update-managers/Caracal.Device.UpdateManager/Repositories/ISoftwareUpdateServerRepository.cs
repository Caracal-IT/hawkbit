using Caracal.Device.UpdateManager.Models.UpdateRequest;

namespace Caracal.Device.UpdateManager.Repositories;

public interface ISoftwareUpdateServerRepository
{
    IAsyncEnumerable<ISoftwareUpdateRequest> GetSoftwareUpdateRequestsAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default);
}