// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.ControllerModels;

public sealed class Polling
{
    [JsonPropertyName("sleep")]
    public required TimeSpan Sleep { get; set; }
}