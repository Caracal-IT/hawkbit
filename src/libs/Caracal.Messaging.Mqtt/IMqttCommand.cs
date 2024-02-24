namespace Caracal.Messaging.Mqtt;

public interface IMqttCommand
{
    Task<string> ExecuteAsync(string topic, string message, CancellationToken cancellationToken);
}