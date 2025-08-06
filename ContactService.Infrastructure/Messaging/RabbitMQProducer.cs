using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using SharedKernel.Messaging.Abstraction;

namespace ContactService.Infrastructure.Messaging;

public class RabbitMQProducer : IEventBus
{
    public async Task PublishAsync<T>(T message, string queueName)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);

        await Task.CompletedTask;
    }
}
