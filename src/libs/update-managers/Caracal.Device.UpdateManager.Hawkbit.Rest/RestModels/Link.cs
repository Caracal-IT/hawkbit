using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels;

public sealed class Link
{
    [JsonPropertyName("href")]
    public required string Href { get; set; }
}