namespace Caracal.Messaging.Mqtt.Client;

public interface IMqttClient
{
    Task StartAsync();
    Task StopAsync();
    Task EnqueueAsync(string topic, string payload);
    Task<ISubscription> SubscribeAsync(string topic);
}