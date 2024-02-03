using Caracal.Device.UpdateManager.Hawkbit.Rest.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddHawkbitClient(this IServiceCollection services)
    {
        /*
        services.AddHttpClient<IHawkbitClient, HawkbitHttpClient>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:8080");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        */

        services.AddHostedService<SoftwareUpdateService>();
        
        return services;
    }
}