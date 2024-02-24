using Caracal.SoftwareUpdate.Application;

namespace Caracal.Device.Manager.Example;

public sealed class Processor: IDisposable
{
    private readonly UpdateRequest _updateRequest;
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    private readonly IEnumerator<(Chunk Chunk, Artifact artifact)> _artifacts;

    private readonly Action _queueResponse;

    private Processor(UpdateRequest updateRequest, CancellationToken cancellationToken)
    {
        _updateRequest = updateRequest;

        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(400));
        _cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([_cancellationTokenSource.Token, _cancellationToken]).Token;
        
        _artifacts = GetArtifacts().GetEnumerator();
        
        _queueResponse = () => Task.Run(OnArtifactReceived, _cancellationToken).ConfigureAwait(false);
    }

    public static async Task ProcessAsync(UpdateRequest request, CancellationToken cancellationToken)
    {
        using var p = new Processor(request, cancellationToken);
        await p.ProcessAsync().ConfigureAwait(false);
    }

    private async Task ProcessAsync()
    {
        Console.WriteLine($"Processing {_updateRequest.Name}");
        
        await RequestUpdateAsync().ConfigureAwait(false);
        _cancellationToken.WaitHandle.WaitOne();
    }
    
    private async Task RequestUpdateAsync()
    {
        await Task.Delay(500, _cancellationToken).ConfigureAwait(false);
        
        if (!_artifacts.MoveNext())
        {
            await _cancellationTokenSource.CancelAsync();
            return;
        }

        var (chunk, artifact) = _artifacts.Current;
        
        Console.WriteLine($"Requesting {chunk.Name} - {artifact.Name}");
        // Mock Callback From Server
        _queueResponse();
        Console.WriteLine($"Done Requesting {chunk.Name} - {artifact.Name}");
    }

    private async Task OnArtifactReceived()
    {
        await Task.Delay(1000, _cancellationToken).ConfigureAwait(false);
        
        var (chunk, artifact) = _artifacts.Current;
        
        Console.WriteLine($"Data received {chunk.Name} - {artifact.Name}");

        await RequestUpdateAsync();
    }

    IEnumerable<(Chunk Chunk, Artifact artifact)> GetArtifacts() {
        foreach (var chunk in _updateRequest.Chunks)
        {
            foreach (var artifact in chunk.Artifacts)
            {
                yield return (chunk, artifact);
            }
        }
    }

    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
        _artifacts.Dispose();
    }
}