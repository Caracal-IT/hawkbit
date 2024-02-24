using Caracal.Messaging.Mqtt.Client;
using Caracal.Messaging.Mqtt.Command;
using Microsoft.Extensions.DependencyInjection;

namespace Caracal.Messaging.Mqtt.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddMqttClient(this IServiceCollection services)
    {
        services.AddSingleton<IMqttClient, MqttClient>();
        services.AddSingleton<IMqttCommandFactory, MqttCommandFactory>();
        
        return services;
    }
}