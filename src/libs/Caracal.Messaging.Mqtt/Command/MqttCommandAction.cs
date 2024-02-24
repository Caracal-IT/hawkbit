using System.Text;
using Caracal.Messaging.Mqtt.Client;
using MQTTnet.Client;
using IMqttClient = Caracal.Messaging.Mqtt.Client.IMqttClient;

namespace Caracal.Messaging.Mqtt.Command;

internal sealed class MqttCommandAction
{
    private readonly CancellationToken _cancellationToken;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly TimeSpan _timeoutTimeSpan = TimeSpan.FromMinutes(3);

    private readonly IMqttClient _mqttClient;
    private ISubscription? _subscription;
    
    private readonly string _topic;
    private readonly string _message;
    private readonly string _responseTopic;
    private string? _response;

    public MqttCommandAction(IMqttClient mqttClient, string topic, string message, string responseTopic, CancellationToken cancellationToken)
    {
        _topic = topic;
        _message = message;
        _responseTopic = responseTopic;
        _mqttClient = mqttClient;
        
        _cancellationTokenSource = new CancellationTokenSource(_timeoutTimeSpan);
        _cancellationToken = CancellationTokenSource.CreateLinkedTokenSource([_cancellationTokenSource.Token, cancellationToken]).Token;
    }

    public async Task<string> ExecuteAsync()
    {
        _subscription = await _mqttClient.SubscribeAsync(_responseTopic).ConfigureAwait(false);
        _subscription.MqttApplicationMessageReceivedEventArgs += SubscriptionOnMqttApplicationMessageReceivedEventArgs;

        await _mqttClient.EnqueueAsync(_topic, _message).ConfigureAwait(false);
        
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