using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Caracal.Messaging.Mqtt.Settings;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using Serilog;

namespace Caracal.Messaging.Mqtt.Client;

internal sealed class MqttClient: IMqttClient, IDisposable
{
    private readonly ILogger _logger;
    private readonly MqttSettings _mqttSettings;
    
    private readonly IManagedMqttClient _mqttClient = new MqttFactory().CreateManagedMqttClient();
    private readonly ConcurrentDictionary<Guid, ISubscription> _subscriptions = [];

    public MqttClient(ILogger logger, IOptions<MqttSettings> mqttSettings)
    {
        _logger = logger;
        _mqttSettings = mqttSettings.Value;
        
        AddMqttEvents();
    }

    public async Task StartAsync()
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(_mqttSettings.TcpServer, _mqttSettings.Port)
            .WithClientId(_mqttSettings.ClientId)
            .WithWillTopic("caracal/status")
            .WithWillPayload("disconnected")
            .WithWillRetain()
            .WithWillMessageExpiryInterval(IMqttClient.DefaultInterval)
            .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
            .WithProtocolVersion(MqttProtocolVersion.V500)
            .Build();
        
        var managedOptions = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(options)
            .Build();
        
        await _mqttClient.StartAsync(managedOptions).ConfigureAwait(false);
        await _mqttClient.EnqueueAsync("caracal/status", "connected").ConfigureAwait(false);
    }

    public async Task StopAsync()
    {
        try
        {
            await _mqttClient.StopAsync().ConfigureAwait(false);
        }
        catch(ObjectDisposedException){}
    }

    public Task EnqueueAsync(string topic, string payload, uint messageExpiryIntervalInSeconds = IMqttClient.DefaultInterval) =>
        EnqueueAsync(topic, payload, string.Empty, messageExpiryIntervalInSeconds);

    public async Task EnqueueAsync(string topic, string payload, string responseTopic, uint messageExpiryIntervalInSeconds = IMqttClient.DefaultInterval) {
        var builder = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload);

        if (!string.IsNullOrWhiteSpace(responseTopic))
            builder = builder.WithResponseTopic(responseTopic);

        if (messageExpiryIntervalInSeconds > 0)
            builder = builder
                .WithRetainFlag()
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtMostOnce)
                .WithMessageExpiryInterval(messageExpiryIntervalInSeconds);

        await _mqttClient.EnqueueAsync(builder.Build()).ConfigureAwait(false);
    }

    public async Task<ISubscription> SubscribeAsync(string topic)
    {
        var subscription = new Subscription( topic, this);
        _subscriptions.TryAdd(subscription.Id, subscription);
        
        await _mqttClient.SubscribeAsync(topic).ConfigureAwait(false);
        await Task.Delay(50).ConfigureAwait(false);
        
        return subscription;
    }

    public Task<string> ExecuteAsync(string topic, string message, string responseTopic, CancellationToken cancellationToken) =>
        new MqttCommand(this, topic, message,  responseTopic, cancellationToken).ExecuteAsync();
    
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
            var regexPattern = $"^{Regex.Escape(template).Replace("\\+", "[^/]+").Replace("\\#", ".+")}$";
            return Regex.IsMatch(topic, regexPattern);
        }
    }

    internal async Task UnsubscribeAsync(ISubscription subscription)
    {
        if (_subscriptions.TryRemove(subscription.Id, out _))
        {
            if(_subscriptions.All(s => s.Value.Topic != subscription.Topic))
                await _mqttClient.UnsubscribeAsync(subscription.Topic).ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        _mqttClient.Dispose();
    }
    
    private void AddMqttEvents()
    {
        _mqttClient.ApplicationMessageReceivedAsync += MqttClientOnApplicationMessageReceivedAsync;
        
        _mqttClient.ConnectedAsync += _ => {
            _logger.Information("Connected to {Server}:{Port}", _mqttSettings.TcpServer, _mqttSettings.Port);
            return Task.CompletedTask;
        };
        
        _mqttClient.DisconnectedAsync += _ => {
            _logger.Warning("Disconnected from {Server}:{Port}", _mqttSettings.TcpServer, _mqttSettings.Port);
            return Task.CompletedTask;
        };
        
        _mqttClient.ConnectingFailedAsync += e => {
            _logger.Error(e.Exception, "Connecting to {Server}:{Port} failed", _mqttSettings.TcpServer, _mqttSettings.Port);
            return Task.CompletedTask;
        };
        
        _mqttClient.ApplicationMessageProcessedAsync += e => {
            _logger.Information("Message processed {Topic}", e.ApplicationMessage.ApplicationMessage.Topic);
            return Task.CompletedTask;
        };
        
        _mqttClient.ApplicationMessageSkippedAsync += e => {
            _logger.Warning("Message skipped {Topic}", e.ApplicationMessage.ApplicationMessage.Topic);
            return Task.CompletedTask;
        };
        
        _mqttClient.ConnectionStateChangedAsync += e => {
            _logger.Information("Connection state changed {State}", _mqttClient.IsConnected ? "Connected" : "Disconnected");
            return Task.CompletedTask;
        };
        
        _mqttClient.ApplicationMessageReceivedAsync += e => {
            _logger.Information("Message received {Topic}", e.ApplicationMessage.Topic);
            return Task.CompletedTask;
        };
    }
}