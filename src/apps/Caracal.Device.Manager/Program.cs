using Caracal.Messaging.Mqtt.Client;
using Caracal.Messaging.Mqtt.Extensions;
using Caracal.SoftwareUpdate.Application.Extensions;

WriteLine("CARACAL - Device Manager");

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient(HttpClients.HawkHttpClient, x =>
{
       x.BaseAddress = new Uri("http://localhost:8080");
       x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("GatewayToken", "2677aea31a570f07d594d3439c1299f2");
});

builder.Services
       .AddMqttClient()
       .AddSoftwareProcessor()
       .AddSoftwareUpdates()
       .AddHawkbitRest();

var host = builder.Build();

var mqtt = host.Services.GetRequiredService<IMqttClient>();

await mqtt.StartAsync();
await host.RunAsync();
await mqtt.StopAsync();