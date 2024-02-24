using System.Text;
using Caracal.Messaging.Mqtt.Client;
using Microsoft.Extensions.Hosting;
using MQTTnet.Client;
using IMqttClient = Caracal.Messaging.Mqtt.Client.IMqttClient;

namespace Caracal.SoftwareUpdate.Application.Services;

public sealed class ResponseRobot(IMqttClient mqttClient): BackgroundService
{
    private ISubscription? _subscription;
    private const int DefaultCommandInterval = 60;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await mqttClient.EnqueueAsync("caracal/services/response-robot", "started");
        
        _subscription = await mqttClient.SubscribeAsync("caracal/actions/software-update/req").ConfigureAwait(false);
        _subscription.MqttApplicationMessageReceivedEventArgs += SubscriptionOnMqttApplicationMessageReceivedEventArgs;
    }

    private async Task SubscriptionOnMqttApplicationMessageReceivedEventArgs(MqttApplicationMessageReceivedEventArgs arg)
    {
        await Task.Delay(millisecondsDelay: Random.Shared.Next(minValue: 0, maxValue: 2000));
        var payload = Encoding.UTF8.GetString(bytes: arg.ApplicationMessage.PayloadSegment);
        await mqttClient.EnqueueAsync(
            topic: arg.ApplicationMessage.ResponseTopic, 
            payload: $"Response -> {payload}",
            messageExpiryIntervalInSeconds: DefaultCommandInterval).ConfigureAwait(false);
    }

    public override Task StopAsync(CancellationToken cancellationToken) => 
        mqttClient.EnqueueAsync("caracal/services/response-robot", "stopped");
}