﻿namespace Caracal.Device.Manager.CommandExample;

public interface IMqttCommand
{
    Task<string> ExecuteAsync(string topic, string message, CancellationToken cancellationToken);
}

internal class MqttCommand(string mqttBroker) : IMqttCommand
{
    public async Task<string> ExecuteAsync(string topic, string message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Broker {mqttBroker}");
        return await new MqttCommandAction(topic, message, cancellationToken)
            .ExecuteAsync()
            .ConfigureAwait(false);
    }
}