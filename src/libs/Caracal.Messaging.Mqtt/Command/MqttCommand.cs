using Caracal.Messaging.Mqtt.Client;

namespace Caracal.Messaging.Mqtt.Command;

internal sealed class MqttCommand(IMqttClient mqttClient) : IMqttCommand
{
    public async Task<string> ExecuteAsync(string topic, string message, string responseTopic, CancellationToken cancellationToken)
    {
        return await new MqttCommandAction(topic, message,  responseTopic, cancellationToken)
            .ExecuteAsync()
            .ConfigureAwait(false);
    }
}