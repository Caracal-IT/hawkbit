using Caracal.Device.Manager.Extensions;
using Caracal.Messaging.Mqtt.Client;
using Caracal.Messaging.Mqtt.Extensions;
using Caracal.SoftwareUpdate.Application.Extensions;

var builder = Host.CreateApplicationBuilder(args)
                  .AddBaseServices();

builder.Services
       .AddSettings()
       .AddMqttClient()
       .AddSoftwareProcessor()
       .AddSoftwareUpdates()
       .AddHawkbitRest();

var host = builder.Build();

var mqtt = host.Services.GetRequiredService<IMqttClient>();

await mqtt.StartAsync();
await host.RunAsync();
await mqtt.StopAsync();