// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.ControllerModels;

public class Config
{
    [JsonPropertyName("polling")]
    public required Polling Polling { get; set; }
}