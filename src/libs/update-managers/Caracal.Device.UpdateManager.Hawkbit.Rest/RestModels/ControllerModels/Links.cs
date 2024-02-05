// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.ControllerModels;

public sealed class Links
{
    [JsonPropertyName("deploymentBase")]
    public Link? DeploymentBase { get; set; }
    
    [JsonPropertyName("installedBase")]
    public Link? InstalledBase { get; set; }
    
    [JsonPropertyName("configData")]
    public Link? ConfigData { get; set; }
}