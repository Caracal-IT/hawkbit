using Caracal.Messaging.Mqtt;
using Caracal.Messaging.Mqtt.Client;
using Caracal.Messaging.Mqtt.Command;
using Caracal.SoftwareUpdate.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Caracal.SoftwareUpdate.Application.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddSoftwareProcessor(this IServiceCollection services)
    {
        services.AddHostedService<ResponseRobot>();
        services.AddHostedService<ProcessUpdateService>();
        
        return services;
    }
}