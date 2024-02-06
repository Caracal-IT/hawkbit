using Caracal.Device.UpdateManager.Hawkbit.RabbitMQ.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Caracal.Device.UpdateManager.Hawkbit.RabbitMQ.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddHawkbitRabbitMQ(this IServiceCollection services)
    {
        //services.AddSingleton<SoftwareUpdateManager>();

        services.AddHostedService<SoftwareUpdateService>();
        
        return services;
    }
}