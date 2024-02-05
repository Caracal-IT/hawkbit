using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Caracal.Device.UpdateManager.Hawkbit.Rest.Constants;
using Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.UpdateRequestsModels;
using Caracal.Device.UpdateManager.Repositories;
using Mapster;

using DomainModels = Caracal.Device.UpdateManager.Models;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.Repositories;

public sealed class SoftwareUpdateServerRepository(IHttpClientFactory httpClientFactory) : ISoftwareUpdateServerRepository
{
    private readonly HttpClient _client = httpClientFactory.CreateClient(HttpClients.HawkHttpClient);
    
    public async IAsyncEnumerable<DomainModels.UpdateRequestsModels.DeploymentRequest> GetDeploymentsAsync(string tenantId, string deviceId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            const string url = "/default/controller/v1/gate1";
            var controller = await _client.GetFromJsonAsync<RestModels.ControllerModels.Controller>(url, cancellationToken).ConfigureAwait(false);

            if(controller != null)
            {
                if (controller.Links.DeploymentBase != null)
                {
                    var deploymentBase = controller.Links.DeploymentBase.Href;
                    var deployment = await _client.GetFromJsonAsync<RestModels.UpdateRequestsModels.DeploymentRequest>(deploymentBase, cancellationToken).ConfigureAwait(false);

                    if (deployment != null)
                        yield return deployment.Adapt<DomainModels.UpdateRequestsModels.DeploymentRequest>();
                }
                
                await Task.Delay(controller.Config.Polling.Sleep, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await Task.Delay(60_000, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    public async Task<byte[]> DownloadArtifactAsync(Models.UpdateRequestsModels.Artifact artifact, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync(artifact.Links.Download.Href, cancellationToken);

        return new byte[0];
    }
}