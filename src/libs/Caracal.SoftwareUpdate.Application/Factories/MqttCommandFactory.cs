using Caracal.Messaging.Mqtt;

namespace Caracal.SoftwareUpdate.Application.Factories;

public sealed class MqttCommandFactory(string broker) : IMqttCommandFactory
{
    private readonly IMqttCommand _mqttCommand = new MqttCommand(broker);

    public IMqttCommand Create() => _mqttCommand;
}