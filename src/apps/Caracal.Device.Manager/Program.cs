using Caracal.Device.Manager;
using Caracal.Device.Manager.CommandExample;
using Caracal.Device.Manager.Example;

WriteLine("CARACAL - Device Manager");

var update = new UpdateRequest(
       Id: Guid.NewGuid(),
       Name: "Request 1",
       Chunks: [
              new Chunk(
                     Name: "Chunk 1",
                     Artifacts: [
                            new Artifact(Name:"Artifact 1"),
                            new Artifact(Name:"Artifact 2"),
                            new Artifact(Name:"Artifact 3")
                     ]
              ),
              new Chunk(
                     Name: "Chunk 2",
                     Artifacts: [
                            new Artifact(Name:"Artifact 4"),
                            new Artifact(Name:"Artifact 5"),
                            new Artifact(Name:"Artifact 6")
                     ]
              )
       ]
);

var cmdFactory = new MqttCommandFactory("Test Broker");
await CommandProcessor.ProcessAsync(cmdFactory, update, CancellationToken.None).ConfigureAwait(false);

//await Processor.ProcessAsync(update, CancellationToken.None);



/*
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
*/