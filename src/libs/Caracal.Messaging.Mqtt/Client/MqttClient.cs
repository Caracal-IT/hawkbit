using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;

namespace Caracal.Messaging.Mqtt.Client;

internal sealed class MqttClient: IMqttClient, IDisposable
{
    private readonly IManagedMqttClient _mqttClient = new MqttFactory().CreateManagedMqttClient();
    private readonly ConcurrentDictionary<Guid, ISubscription> _subscriptions = [];

    public MqttClient()
    {
        _mqttClient.ApplicationMessageReceivedAsync += MqttClientOnApplicationMessageReceivedAsync;
        _mqttClient.ConnectingFailedAsync += args => { Console.WriteLine("Failed to Connect"); return Task.CompletedTask; };
    }

    public async Task StartAsync()
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883) // Replace with your MQTT broker address
            .WithClientId("Client1")
            .WithWillTopic("caracal/status")
            .WithWillPayload("disconnected")
            .WithWillRetain()
            .WithProtocolVersion(MqttProtocolVersion.V500)
            .Build();
        
        var managedOptions = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(options)
            .Build();
        
        await _mqttClient.StartAsync(managedOptions);

        await _mqttClient.EnqueueAsync("caracal/status", "connected");
    }

    public async Task StopAsync() => await _mqttClient.StopAsync();

    public async Task EnqueueAsync(string topic, string payload)
    {
        var msg = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            //.WithExactlyOnceQoS()
            .Build();

        await _mqttClient.EnqueueAsync(msg);
    }

    public async Task<ISubscription> SubscribeAsync(string topic)
    {
        var subscription = new Subscription( topic, this);
        _subscriptions.TryAdd(subscription.Id, subscription);
        
        await _mqttClient.SubscribeAsync(topic);
        await Task.Delay(10);
        
        return subscription;
    }

    private async Task MqttClientOnApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
    {
        if (_subscriptions.IsEmpty)
            return;
        
        var subscriptions = _subscriptions
            .Where(s => ValidateTopic(s.Value.Topic, arg.ApplicationMessage.Topic))
            .Select(s => s.Value.OnMqttApplicationMessageReceivedEventArgs(arg));

        await Task.WhenAll(subscriptions).ConfigureAwait(false);
        return;

        static bool ValidateTopic(string template, string topic)
        {
            // Escape special characters and replace wildcards with regex equivalents
            var regexPattern = "^" + Regex.Escape(template).Replace("\\+", "[^/]+").Replace("\\#", ".+") + "$";
            return Regex.IsMatch(topic, regexPattern);
        }
    }

    internal async Task UnsubscribeAsync(ISubscription subscription)
    {
        if (_subscriptions.TryRemove(subscription.Id, out _))
        {
            if(_subscriptions.All(s => s.Value.Topic != subscription.Topic))
                await _mqttClient.UnsubscribeAsync(subscription.Topic);
        }
    }

    public void Dispose()
    {
        _mqttClient.ApplicationMessageReceivedAsync -= MqttClientOnApplicationMessageReceivedAsync;
        _mqttClient.Dispose();
    }
}