using System.Net;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using Caracal.Device.UpdateManager.Hawkbit.Rest.Constants;
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
            var url = $"/{tenantId}/controller/v1/{deviceId}";
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

    public async Task<byte[]> DownloadArtifactAsync(Models.UpdateRequestsModels.Artifact artifact, CancellationToken cancellationToken = default) =>
        await _client.GetByteArrayAsync(artifact.Links.Download.Href, cancellationToken);

    public async Task<bool> UpdateStatusAsync(string tenantId, string deviceId, int deploymentId, CancellationToken cancellationToken = default)
    {
        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.ffff");
        var url = $"/{tenantId}/controller/v1/{deviceId}/deploymentBase/{deploymentId}/feedback";
        
        var request = $$"""
                        {
                          "id": "{{deploymentId}}",
                          "time": "{{timestamp}}",
                          "status": {
                            "execution0": "downloaded",
                            "execution": "closed",
                            "result": {
                              "finished": "success",
                              "progress": {
                                "cnt": 1,
                                "of": 5
                              }
                            },
                            "code": 200,
                            "details": [
                              "string"
                            ]
                          }
                        }
                        """;

        var content = new StringContent(request, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

        return response.StatusCode == HttpStatusCode.OK;
    }
}