namespace Caracal.Device.Manager.CommandExample;

public class MqttCommandFactory(string broker) : IMqttCommandFactory
{
    private readonly IMqttCommand _mqttCommand = new MqttCommand(broker);

    public IMqttCommand Create() => _mqttCommand;
}