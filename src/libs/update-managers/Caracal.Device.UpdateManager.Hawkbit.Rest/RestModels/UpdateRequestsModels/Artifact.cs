// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.UpdateRequestsModels;

public class Artifact
{
    [JsonPropertyName("filename")]
    public required string Filename { get; set; }
    
    [JsonPropertyName("hashes")]
    public required Hashes Hashes { get; set; }
    
    [JsonPropertyName("size")]
    public required int Size { get; set; }
    
    [JsonPropertyName("_links")]
    public required Links Links { get; set; }
}