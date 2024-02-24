namespace Caracal.Messaging.Mqtt.Command;

internal sealed class MqttCommandAction
{
    private readonly Action _queueResponse;
    
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TimeSpan _timeoutTimeSpan = TimeSpan.FromMinutes(3);

    private readonly string _topic;
    private readonly string _message;
    private readonly string _responseTopic;
    private string? _response;

    public MqttCommandAction(string topic, string message, string responseTopic, CancellationToken cancellationToken)
    {
        _topic = topic;
        _message = message;
        _responseTopic = responseTopic;
        
        _queueResponse = () => Task.Run(OnResponseReceivedAsync, _cancellationToken).ConfigureAwait(false);
        
        _cancellationTokenSource = new CancellationTokenSource(_timeoutTimeSpan);
        _cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([_cancellationTokenSource.Token, cancellationToken]).Token;
    }

    public Task<string> ExecuteAsync()
    {
        Console.WriteLine($"Executing {_message} on {_topic}");
        _queueResponse();
        Console.WriteLine($"Done Executing {_message}");

        _cancellationToken.WaitHandle.WaitOne(_timeoutTimeSpan);

        if (_response is null)
            throw new TimeoutException();
        
        return Task.FromResult(_response);
    }

    private async Task OnResponseReceivedAsync()
    {
        await Task.Delay(Random.Shared.Next(500, 1_000) , _cancellationToken).ConfigureAwait(false);
        Console.WriteLine($"Message Received {_message}");

        _response = $"Response {_message}";

        await _cancellationTokenSource.CancelAsync().ConfigureAwait(false);
    }
}