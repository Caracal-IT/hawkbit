WriteLine("CARACAL - Device Manager");

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHawkbitClient();

var host = builder.Build();
await host.RunAsync();