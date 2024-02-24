namespace Caracal.Messaging.Mqtt.Client;

public interface IMqttClient
{
    internal const int DefaultInterval = 3_600;
    internal const int DefaultCommandInterval = 60;
    
    Task StartAsync();
    Task StopAsync();
    Task EnqueueAsync(string topic, string payload, uint messageExpiryIntervalInSeconds = DefaultInterval);
    Task EnqueueAsync(string topic, string payload, string responseTopic, uint messageExpiryIntervalInSeconds = DefaultInterval);
    Task<ISubscription> SubscribeAsync(string topic);
    Task<string> ExecuteAsync(string topic, string message, string responseTopic, CancellationToken cancellationToken);
}