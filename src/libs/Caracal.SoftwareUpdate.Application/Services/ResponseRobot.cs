using Caracal.Messaging.Mqtt.Client;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client;
using IMqttClient = Caracal.Messaging.Mqtt.Client.IMqttClient;

namespace Caracal.SoftwareUpdate.Application.Services;

public sealed class ResponseRobot(IMqttClient mqttClient): BackgroundService
{
    private ISubscription? _subscription;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await mqttClient.EnqueueAsync("caracal/services/response-robot", "started");
        
        _subscription = await mqttClient.SubscribeAsync("test1");
        _subscription.MqttApplicationMessageReceivedEventArgs += SubscriptionOnMqttApplicationMessageReceivedEventArgs;
    }

    private Task SubscriptionOnMqttApplicationMessageReceivedEventArgs(MqttApplicationMessageReceivedEventArgs arg)
    {
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken) => 
        await mqttClient.EnqueueAsync("caracal/services/response-robot", "stopped");
}