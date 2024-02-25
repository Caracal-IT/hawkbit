using System.Text.Json;
using Caracal.Messaging.Mqtt.Client;
using Caracal.SoftwareUpdate.Application.Data;
using Caracal.SoftwareUpdate.Application.Processors;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Caracal.SoftwareUpdate.Application.Services;

public sealed class ProcessUpdateService(IMqttClient mqttClient, ILogger logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await mqttClient.EnqueueAsync("caracal/services/software-update", "started").ConfigureAwait(false);
        
        var useCommand = false;
        await Task.Delay(1_000, stoppingToken);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            useCommand = !useCommand;
            
            var req = await CreateDemoUpdateRequestAsync(stoppingToken).ConfigureAwait(false);
            var newReq = req with {
                Id = Guid.NewGuid(),
                Name = $"Req {Guid.NewGuid()}"
            };
            
            logger.Information("Start {Options} ...", useCommand?"With Command":string.Empty);
            await Task.Delay(1_000, stoppingToken);
            
            if(useCommand) await CommandProcessor.ProcessAsync(mqttClient, newReq, logger, stoppingToken).ConfigureAwait(false);
            else await Processor.ProcessAsync(newReq, logger, stoppingToken).ConfigureAwait(false);
            
            logger.Information("Done ....");
            
            await Task.Delay(10_000, stoppingToken).ConfigureAwait(false);
        }
    }
    
    public override Task StopAsync(CancellationToken cancellationToken) => 
        mqttClient.EnqueueAsync("caracal/services/software-update", "stopped");

    private static async Task<UpdateRequest> CreateDemoUpdateRequestAsync(CancellationToken cancellationToken)
    {
        await using var fs = File.OpenRead("Data/update-request.json");
        return (await JsonSerializer.DeserializeAsync<UpdateRequest>(fs, cancellationToken: cancellationToken).ConfigureAwait(false))!;
    }
}