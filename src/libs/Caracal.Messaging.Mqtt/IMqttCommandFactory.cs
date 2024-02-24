namespace Caracal.Messaging.Mqtt;

public interface IMqttCommandFactory
{
    IMqttCommand Create();
}