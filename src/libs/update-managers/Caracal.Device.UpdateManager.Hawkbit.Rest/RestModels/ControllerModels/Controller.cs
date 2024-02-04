using System.Text.Json.Serialization;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.RestModels.ControllerModels;

public class Controller
{
    [JsonPropertyName("config")]
    public required Config Config { get; set; }
    
    [JsonPropertyName("_links")]
    public required Links Links { get; set; }
}