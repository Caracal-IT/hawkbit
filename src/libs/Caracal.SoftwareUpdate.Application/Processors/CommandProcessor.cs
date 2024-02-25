using Caracal.Messaging.Mqtt.Client;
using Caracal.SoftwareUpdate.Application.Data;
using Serilog;

namespace Caracal.SoftwareUpdate.Application.Processors;

public sealed class CommandProcessor: IDisposable
{
    private readonly UpdateRequest _updateRequest;
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenSource _cancellationTokenSource;

    private readonly IMqttClient _mqttClient;
    private readonly ILogger _logger;

    private CommandProcessor(IMqttClient mqttClient, UpdateRequest updateRequest, ILogger logger, CancellationToken cancellationToken)
    {
        _mqttClient = mqttClient;
        _updateRequest = updateRequest;
        _logger = logger;

        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(400));
        _cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([_cancellationTokenSource.Token, cancellationToken]).Token;
    }

    public static async Task ProcessAsync(IMqttClient mqttClient, UpdateRequest request, ILogger logger, CancellationToken cancellationToken)
    {
        using var p = new CommandProcessor(mqttClient, request, logger, cancellationToken);
        await p.ProcessAsync().ConfigureAwait(false);
    }

    private async Task ProcessAsync()
    {
        _logger.Information("Processing {Name}", _updateRequest.Name);
        
        foreach (var chunk in _updateRequest.Chunks)
        foreach (var artifact in chunk.Artifacts)
        {
            await DownloadAsync(chunk, artifact).ConfigureAwait(false);
        }
    }

    private async Task DownloadAsync(Chunk chunk, Artifact artifact)
    {
        var msg = $"Requesting {chunk.Name} - {artifact.Name}";
        _logger.Information("Begin - {Message}", msg);
        
        var response = await _mqttClient.ExecuteAsync(
            topic: "caracal/actions/software-update/req", 
            message: msg, 
            responseTopic: "caracal/actions/software-update/resp", 
            cancellationToken: _cancellationToken).ConfigureAwait(false);
        
        _logger.Information("End - {Message} --> {Response}", msg, response);
    }

    public void Dispose() => _cancellationTokenSource.Dispose();
}