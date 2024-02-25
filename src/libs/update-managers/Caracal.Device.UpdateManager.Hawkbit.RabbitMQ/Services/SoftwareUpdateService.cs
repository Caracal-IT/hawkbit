using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace Caracal.Device.UpdateManager.Hawkbit.RabbitMQ.Services;

public class SoftwareUpdateService(ILogger logger): BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //await AddRabbit();
            Rabbit2();
            logger.Information("Rabbit MQ");
            await Task.Delay(30000, stoppingToken);
        }
    }

    private void Rabbit2()
    {
        var factory = new ConnectionFactory
        {
            HostName = "127.0.0.1",
            Port = 5672,
            UserName = "admin",
            Password = "admin"
        }; // Replace with your RabbitMQ server hostname
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateChannel();
        
        // Declare the queue
        channel.QueueDeclare(queue: "hawkbit_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        // Set up the consumer
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            logger.Information("Received message: {0}", message);
            // Process the message as needed (e.g., trigger software update)
        };
        channel.BasicConsume(queue: "hawkbit_queue", autoAck: true, consumer: consumer);

        logger.Information("Waiting for messages...");
        Console.ReadLine();
    }

    private async Task AddRabbit()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = "127.0.0.1",
                Port = 5672,
                UserName = "admin",
                Password = "admin"
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            channel.QueueDeclare(queue: "hello",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            const string message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);


            await channel.BasicPublishAsync(exchange: "",
                routingKey: "mqtt-subscription-mqtt-explorer-b3d6b461qos0",
                //basicProperties: null,
                body: body);
            
            await channel.CloseAsync();

            logger.Information($" [x] Sent {message}");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "{Exception}", ex.Message);
            
            if(ex.InnerException is not null)
                logger.Error(ex.InnerException, "{Exception}", ex.InnerException?.Message);
        }
    }
}