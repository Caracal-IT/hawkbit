namespace Caracal.Messaging.Mqtt.Command;

public interface IMqttCommandFactory
{
    IMqttCommand Create();
}