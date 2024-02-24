using System.Text.Json;
using Caracal.Messaging.Mqtt;
using Caracal.Messaging.Mqtt.Client;
using Caracal.SoftwareUpdate.Application.Data;
using Caracal.SoftwareUpdate.Application.Processors;
using Microsoft.Extensions.Hosting;

namespace Caracal.SoftwareUpdate.Application.Services;

public sealed class ProcessUpdateService(IMqttClient mqttClient) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await mqttClient.EnqueueAsync("caracal/services/software-update", "started");
        
        var useCommand = false;
        await Task.Delay(1_000, stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            useCommand = !useCommand;
            
            var req = await CreateDemoUpdateRequestAsync(stoppingToken);
            var newReq = req with {
                Id = Guid.NewGuid(),
                Name = $"Req {Guid.NewGuid()}"
            };
            
            Console.Clear();
            Console.WriteLine($"Start {(useCommand?"With Command":string.Empty)} ...");
            await Task.Delay(1_000, stoppingToken);
            
            if(useCommand) await CommandProcessor.ProcessAsync(mqttClient, newReq, stoppingToken);
            else await Processor.ProcessAsync(newReq, stoppingToken);
            
            Console.WriteLine("Done ....");
            
            await Task.Delay(10_000, stoppingToken);
        }
    }
    
    public override async Task StopAsync(CancellationToken cancellationToken) => 
        await mqttClient.EnqueueAsync("caracal/services/software-update", "stopped");

    private static async Task<UpdateRequest> CreateDemoUpdateRequestAsync(CancellationToken cancellationToken)
    {
        await using var fs = File.OpenRead("Data/update-request.json");
        return (await JsonSerializer.DeserializeAsync<UpdateRequest>(fs, cancellationToken: cancellationToken))!;
    }
}