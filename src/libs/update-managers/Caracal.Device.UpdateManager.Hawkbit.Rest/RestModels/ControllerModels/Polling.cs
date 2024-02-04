// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.ControllerModels;

public class Polling
{
    [JsonPropertyName("sleep")]
    public required TimeSpan Sleep { get; set; }
}