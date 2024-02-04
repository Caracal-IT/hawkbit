// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.UpdateRequestsModels;

public class Chunk
{
    [JsonPropertyName("part")]
    public required string Part { get; set; }
    
    [JsonPropertyName("version")]
    public required string Version { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("artifacts")]
    public required List<Artifact> Artifacts { get; set; }
}