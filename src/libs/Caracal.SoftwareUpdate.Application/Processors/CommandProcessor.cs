using Caracal.Messaging.Mqtt.Client;
using Caracal.SoftwareUpdate.Application.Data;

namespace Caracal.SoftwareUpdate.Application.Processors;

public sealed class CommandProcessor: IDisposable
{
    private readonly UpdateRequest _updateRequest;
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenSource _cancellationTokenSource;

    private readonly IMqttClient _mqttClient;

    private CommandProcessor(IMqttClient mqttClient, UpdateRequest updateRequest, CancellationToken cancellationToken)
    {
        _mqttClient = mqttClient;
        _updateRequest = updateRequest;

        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(400));
        _cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([_cancellationTokenSource.Token, cancellationToken]).Token;
    }

    public static async Task ProcessAsync(IMqttClient mqttClient, UpdateRequest request, CancellationToken cancellationToken)
    {
        using var p = new CommandProcessor(mqttClient, request, cancellationToken);
        await p.ProcessAsync().ConfigureAwait(false);
    }

    private async Task ProcessAsync()
    {
        Console.WriteLine($"Processing {_updateRequest.Name}");
        
        foreach (var chunk in _updateRequest.Chunks)
        foreach (var artifact in chunk.Artifacts)
        {
            await DownloadAsync(chunk, artifact).ConfigureAwait(false);
        }
    }

    private async Task DownloadAsync(Chunk chunk, Artifact artifact)
    {
        var msg = $"Requesting {chunk.Name} - {artifact.Name}";
        Console.WriteLine($"Begin - {msg}");
        
        var result = await _mqttClient.ExecuteAsync(
            topic: "caracal/actions/software-update/req", 
            message: msg, 
            responseTopic: "caracal/actions/software-update/resp", 
            cancellationToken: _cancellationToken).ConfigureAwait(false);
        
        Console.WriteLine($"End - {msg} --> {result}");
    }

    public void Dispose() => _cancellationTokenSource.Dispose();
}