// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;
using Caracal.Device.UpdateManager.Models.UpdateRequest;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.UpdateRequestsModels;

public class DeploymentRequest: ISoftwareUpdateRequest
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }
    
    [JsonPropertyName("deployment")]
    public required Deployment Deployment { get; set; }
}