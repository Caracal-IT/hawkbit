using System.Text;
using MQTTnet.Client;

namespace Caracal.Messaging.Mqtt.Client;

internal sealed class MqttCommand
{
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TimeSpan _timeoutTimeSpan = TimeSpan.FromMinutes(3);

    private readonly IMqttClient _mqttClient;
    private ISubscription? _subscription;
    
    private readonly string _topic;
    private readonly string _payload;
    private readonly string _responseTopic;
    private string? _response;

    public MqttCommand(IMqttClient mqttClient, string topic, string payload, string responseTopic, CancellationToken cancellationToken)
    {
        _topic = topic;
        _payload = payload;
        _responseTopic = $"{responseTopic}@{Guid.NewGuid()}";
        
        _mqttClient = mqttClient;
        
        _cancellationTokenSource = new CancellationTokenSource(_timeoutTimeSpan);
        _cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([_cancellationTokenSource.Token, cancellationToken]).Token;
    }

    public async Task<string> ExecuteAsync()
    {
        _subscription = await _mqttClient.SubscribeAsync(_responseTopic).ConfigureAwait(false);
        _subscription.MqttApplicationMessageReceivedEventArgs += SubscriptionOnMqttApplicationMessageReceivedEventArgs;

        await _mqttClient.EnqueueAsync(_topic, _payload, _responseTopic, messageExpiryIntervalInSeconds: IMqttClient.DefaultCommandInterval)
                         .ConfigureAwait(false);
        
        _cancellationToken.WaitHandle.WaitOne(_timeoutTimeSpan);

        if (_response is null)
            throw new TimeoutException();
        
        return _response;
    }

    private async Task SubscriptionOnMqttApplicationMessageReceivedEventArgs(MqttApplicationMessageReceivedEventArgs arg)
    {
        _response = Encoding.UTF8.GetString(arg.ApplicationMessage.PayloadSegment);
        
        await _subscription!.UnsubscribeAsync().ConfigureAwait(false);
        await _cancellationTokenSource.CancelAsync().ConfigureAwait(false);
    }
}