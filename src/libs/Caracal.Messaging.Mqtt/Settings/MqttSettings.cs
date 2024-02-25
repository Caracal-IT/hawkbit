using System.ComponentModel.DataAnnotations;

namespace Caracal.Messaging.Mqtt.Settings;

public class MqttSettings
{
    [Required(ErrorMessage = "TcpServer is required")]
    public required string TcpServer { get; set; }
    
    [Range(1000, 9999)]
    public int Port { get; set; }
    
    [Required(ErrorMessage = "ClientId is required")]
    public required string  ClientId { get; set; }
}