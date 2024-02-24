using System.Text;
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
        
        _subscription = await mqttClient.SubscribeAsync("caracal/actions/software-update/req");
        _subscription.MqttApplicationMessageReceivedEventArgs += SubscriptionOnMqttApplicationMessageReceivedEventArgs;
    }

    private async Task SubscriptionOnMqttApplicationMessageReceivedEventArgs(MqttApplicationMessageReceivedEventArgs arg)
    {
        await Task.Delay(Random.Shared.Next(0, 2000));
        var payload = Encoding.UTF8.GetString(arg.ApplicationMessage.PayloadSegment);
        await mqttClient.EnqueueAsync("caracal/actions/software-update/resp", $"Response -> {payload}");
    }

    public override async Task StopAsync(CancellationToken cancellationToken) => 
        await mqttClient.EnqueueAsync("caracal/services/response-robot", "stopped");
}