using MQTTnet.Client;

namespace Caracal.Messaging.Mqtt.Client;

internal sealed class Subscription(string topic, MqttClient mqttClient): ISubscription
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Topic { get; } = topic;
    
    public event Func<MqttApplicationMessageReceivedEventArgs, Task>? MqttApplicationMessageReceivedEventArgs;
    
    public async Task OnMqttApplicationMessageReceivedEventArgs(MqttApplicationMessageReceivedEventArgs arg)
    {
        if(MqttApplicationMessageReceivedEventArgs is not null)
            await MqttApplicationMessageReceivedEventArgs(arg);
    }

    public Task UnsubscribeAsync() => 
        mqttClient.UnsubscribeAsync(this);
}