namespace Caracal.Messaging.Mqtt.Command;

public interface IMqttCommand
{
    Task<string> ExecuteAsync(string topic, string message, string responseTopic, CancellationToken cancellationToken);
}