using MQTTnet.Client;

namespace Caracal.Messaging.Mqtt.Client;

public interface ISubscription
{
    event Func<MqttApplicationMessageReceivedEventArgs, Task> MqttApplicationMessageReceivedEventArgs;

    Guid Id { get; }
    
    string Topic { get; }
    
    internal Task OnMqttApplicationMessageReceivedEventArgs(MqttApplicationMessageReceivedEventArgs arg);

    Task UnsubscribeAsync();
}