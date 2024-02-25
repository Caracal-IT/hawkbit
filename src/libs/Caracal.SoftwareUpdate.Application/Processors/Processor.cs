using Caracal.SoftwareUpdate.Application.Data;
using Serilog;

namespace Caracal.SoftwareUpdate.Application.Processors;

public sealed class Processor: IDisposable
{
    private readonly ILogger _logger;
    private readonly UpdateRequest _updateRequest;
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenSource _cancellationTokenSource;
    
    private readonly IEnumerator<(Chunk Chunk, Artifact artifact)> _artifacts;

    private readonly Action _queueResponse;

    private Processor(UpdateRequest updateRequest, ILogger logger, CancellationToken cancellationToken)
    {
        _updateRequest = updateRequest;
        _logger = logger.ForContext<Processor>();

        _cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(400));
        _cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([_cancellationTokenSource.Token, _cancellationToken]).Token;
        
        _artifacts = GetArtifacts().GetEnumerator();
        
        _queueResponse = () => Task.Run(OnArtifactReceived, _cancellationToken).ConfigureAwait(false);
    }

    public static async Task ProcessAsync(UpdateRequest request, ILogger logger, CancellationToken cancellationToken)
    {
        using var p = new Processor(request, logger, cancellationToken);
        await p.ProcessAsync().ConfigureAwait(false);
    }

    private async Task ProcessAsync()
    {
        _logger.Information("Processing {Name}",_updateRequest.Name);
        
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
        
        _logger.Information("Requesting {Chunk} - {Artifact}", chunk.Name, artifact.Name);
        
         // Mock Callback From Server
        _queueResponse();
        _logger.Information("Done Requesting {Chunk} - {Artifact}", chunk.Name, artifact.Name);
    }

    private async Task OnArtifactReceived()
    {
        await Task.Delay(1000, _cancellationToken).ConfigureAwait(false);
        
        var (chunk, artifact) = _artifacts.Current;
        _logger.Information("Data received {Chunk} - {Artifact}", chunk.Name, artifact.Name);

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