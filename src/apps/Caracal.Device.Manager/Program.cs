using Caracal.Device.UpdateManager.Extensions;
using Caracal.Device.UpdateManager.Hawkbit.Rest.Extensions;

WriteLine("CARACAL - Device Manager");

var builder = Host.CreateApplicationBuilder(args);

builder.Services
       .AddSoftwareUpdates()
       .AddHawkbitRest();


var host = builder.Build();
await host.RunAsync();