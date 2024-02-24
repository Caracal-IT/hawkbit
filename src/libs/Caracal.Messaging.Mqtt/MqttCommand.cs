namespace Caracal.Messaging.Mqtt;

internal sealed class MqttCommand(string mqttBroker) : IMqttCommand
{
    public async Task<string> ExecuteAsync(string topic, string message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Broker {mqttBroker}");
        return await new MqttCommandAction(topic, message, cancellationToken)
            .ExecuteAsync()
            .ConfigureAwait(false);
    }
}