// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.UpdateRequestsModels;

public sealed class DeploymentRequest
{
    [JsonPropertyName("id")]
    public required int Id { get; set; }
    
    [JsonPropertyName("deployment")]
    public required Deployment Deployment { get; set; }
}