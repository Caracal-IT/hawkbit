using System.Text.Json;
using Caracal.Messaging.Mqtt;
using Caracal.SoftwareUpdate.Application.Data;
using Caracal.SoftwareUpdate.Application.Processors;
using Microsoft.Extensions.Hosting;

namespace Caracal.SoftwareUpdate.Application.Services;

public sealed class ProcessUpdateService(IMqttCommandFactory mqttCommandFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
            
            if(useCommand) await CommandProcessor.ProcessAsync(mqttCommandFactory, newReq, stoppingToken);
            else await Processor.ProcessAsync(newReq, stoppingToken);
            
            Console.WriteLine("Done ....");
            
            await Task.Delay(10_000, stoppingToken);
        }
    }

    private static async Task<UpdateRequest> CreateDemoUpdateRequestAsync(CancellationToken cancellationToken)
    {
        await using var fs = File.OpenRead("Data/update-request.json");
        return (await JsonSerializer.DeserializeAsync<UpdateRequest>(fs, cancellationToken: cancellationToken))!;
    }
}