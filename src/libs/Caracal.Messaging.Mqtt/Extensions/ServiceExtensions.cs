using Caracal.Messaging.Mqtt.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Caracal.Messaging.Mqtt.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddMqttClient(this IServiceCollection services)
    {
        services.AddSingleton<IMqttClient, MqttClient>();
        
        return services;
    }
}