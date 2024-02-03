namespace Caracal.Device.UpdateManager.Repositories;

public interface ISoftwareUpdateServerRepository
{
    Task<string> GetSoftwareUpdateServerUrlAsync(CancellationToken cancellationToken = default);
}