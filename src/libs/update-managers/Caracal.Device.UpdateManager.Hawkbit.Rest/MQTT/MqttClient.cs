using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Caracal.Device.UpdateManager.Hawkbit.Rest.MQTT;

public class MqttClient
{
    private IManagedMqttClient _mqttClient;

    public MqttClient()
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883) // Replace with your MQTT broker address
            .WithClientId("Client1")
            .Build();

        var managedOptions = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(options)
            .Build();

        _mqttClient = new MqttFactory().CreateManagedMqttClient();
    }

    public async Task StartAsync()
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883) // Replace with your MQTT broker address
            .WithClientId("Client1")
            .Build();
        
        var managedOptions = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(options)
            .Build();
        
        await _mqttClient.StartAsync(managedOptions);
    }

    public async Task EnqueueAsync(string topic, string message)
    {
        var msg = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(message)
            //.WithExactlyOnceQoS()
            .Build();

        await _mqttClient.EnqueueAsync(msg);
    }
    
    public async Task Test()
    {
        var options = new MqttClientOptionsBuilder()
            .WithTcpServer("localhost", 1883) // Replace with your MQTT broker address
            .WithClientId("Client1")
            .Build();

        var managedOptions = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(options)
            .Build();

        var mqttClient = new MqttFactory().CreateManagedMqttClient();

       /* mqttClient.UseApplicationMessageReceivedHandler(e =>
        {
            Console.WriteLine($"Received message from topic: {e.ApplicationMessage.Topic}");
            Console.WriteLine($"Message payload: {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
        });
        */

       mqttClient.ApplicationMessageReceivedAsync += args =>
       {
           Console.WriteLine(args.ApplicationMessage.ConvertPayloadToString());
           return Task.CompletedTask;
       };

       await mqttClient.StartAsync(managedOptions);

        await mqttClient.SubscribeAsync("test/topic");

        var message = new MqttApplicationMessageBuilder()
            .WithTopic("test/topic")
            .WithPayload("Hello, MQTT!")
            //.WithExactlyOnceQoS()
            .Build();

        await mqttClient.EnqueueAsync(message);

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();

        await mqttClient.StopAsync();
    }
}