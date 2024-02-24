using Caracal.Messaging.Mqtt;
using Caracal.SoftwareUpdate.Application.Data;

namespace Caracal.SoftwareUpdate.Application.Processors;

public sealed class CommandProcessor: IDisposable
{
    private readonly UpdateRequest _updateRequest;
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenSource _cancellationTokenSource;

    private readonly IMqttCommandFactory _cmdFactory;

    private CommandProcessor(IMqttCommandFactory factory, UpdateRequest updateRequest, CancellationToken cancellationToken)
    {
        _cmdFactory = factory;
        _updateRequest = updateRequest;

        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(400));
        _cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([_cancellationTokenSource.Token, cancellationToken]).Token;
    }

    public static async Task ProcessAsync(IMqttCommandFactory factory, UpdateRequest request, CancellationToken cancellationToken)
    {
        using var p = new CommandProcessor(factory, request, cancellationToken);
        await p.ProcessAsync().ConfigureAwait(false);
    }

    private async Task ProcessAsync()
    {
        Console.WriteLine($"Processing {_updateRequest.Name}");
        
        foreach (var chunk in _updateRequest.Chunks)
        foreach (var artifact in chunk.Artifacts)
        {
            await DownLoadAsync(chunk, artifact).ConfigureAwait(false);
        }
    }

    private async Task DownLoadAsync(Chunk chunk, Artifact artifact)
    {
        var msg = $"Requesting {chunk.Name} - {artifact.Name}";
        var cmd = _cmdFactory.Create();
        Console.WriteLine($"Begin - ${msg}");
        var result = await cmd.ExecuteAsync("my-topic", msg, _cancellationToken).ConfigureAwait(false);
        Console.WriteLine($"End - {msg} --> {result}");
    }

    public void Dispose() => _cancellationTokenSource.Dispose();
}