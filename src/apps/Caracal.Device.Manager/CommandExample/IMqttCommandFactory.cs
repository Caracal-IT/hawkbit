namespace Caracal.Device.Manager.CommandExample;

public interface IMqttCommandFactory
{
    IMqttCommand Create();
}