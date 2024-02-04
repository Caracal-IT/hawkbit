// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.UpdateRequestsModels;

public class Hashes
{
    [JsonPropertyName("sha1")]
    public required string Sha1 { get; set; }
    
    [JsonPropertyName("md5")]
    public required string Md5 { get; set; }
    
    [JsonPropertyName("sha256")]
    public required string Sha256 { get; set; }
}