WriteLine("CARACAL - Device Manager");

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient(HttpClients.HawkHttpClient, x =>
{
       x.BaseAddress = new Uri("http://localhost:8080");
       //x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("GatewayToken", "d25c551dedcaeb6bc23705b445a8403b");
       x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("GatewayToken", "2677aea31a570f07d594d3439c1299f2");
});

builder.Services
       .AddSoftwareUpdates()
       .AddHawkbitRest();


var host = builder.Build();
await host.RunAsync();