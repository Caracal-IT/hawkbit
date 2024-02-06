using Caracal.Device.UpdateManager.Managers;
using Caracal.Device.UpdateManager.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Caracal.Device.UpdateManager.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddSoftwareUpdates(this IServiceCollection services)
    {
        //services.AddSingleton<SoftwareUpdateManager>();

        //services.AddHostedService<SoftwareUpdateService>();

        services.AddHostedService<UpdateService>();
        
        return services;
    }
}