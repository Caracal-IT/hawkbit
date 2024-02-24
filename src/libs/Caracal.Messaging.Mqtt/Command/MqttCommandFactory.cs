using Caracal.Messaging.Mqtt.Client;

namespace Caracal.Messaging.Mqtt.Command;

internal sealed class MqttCommandFactory(IMqttClient mqttClient) : IMqttCommandFactory
{
    private readonly IMqttCommand _mqttCommand = new MqttCommand(mqttClient);

    public IMqttCommand Create() => _mqttCommand;
}