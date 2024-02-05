// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.UpdateRequestsModels;

public sealed class Links
{
    [JsonPropertyName("download-http")]
    public required Link Download { get; set; }

    [JsonPropertyName("md5sum-http")]
    public required Link Md5Sum { get; set; }
}