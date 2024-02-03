using Caracal.Device.UpdateManager.Repositories;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.Repositories;

public sealed class SoftwareUpdateServerRepository : ISoftwareUpdateServerRepository
{
    public Task<string> GetSoftwareUpdateServerUrlAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult("http://localhost:8080");
    }
}