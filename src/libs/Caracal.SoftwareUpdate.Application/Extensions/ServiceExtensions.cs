using Caracal.Messaging.Mqtt;
using Caracal.SoftwareUpdate.Application.Factories;
using Caracal.SoftwareUpdate.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Caracal.SoftwareUpdate.Application.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddSoftwareProcessor(this IServiceCollection services)
    {
        services.AddSingleton<IMqttCommandFactory>(new MqttCommandFactory("Test Broker"));
        
        services.AddHostedService<ProcessUpdateService>();
        
        return services;
    }
}