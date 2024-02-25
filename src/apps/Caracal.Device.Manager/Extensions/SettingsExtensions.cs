using Caracal.Messaging.Mqtt.Settings;

namespace Caracal.Device.Manager.Extensions;

public static class SettingsExtensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services.AddOptions<MqttSettings>("Mqtt");
        
        return services;
    }

    private static IServiceCollection AddOptions<T>(this IServiceCollection services, string sectionPath)
    {
        services.AddOptions<MqttSettings>()
                .BindConfiguration("Mqtt")
                .ValidateDataAnnotations();

        return services;
    }
}