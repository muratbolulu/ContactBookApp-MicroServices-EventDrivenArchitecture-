using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using SharedKernel.Messaging;

namespace ContactService.Infrastructure.Messaging;

public class RabbitMQProducer : IEventBus
{
    private readonly IConnection _connection;

    public RabbitMQProducer()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnection();
    }

    public Task PublishAsync<T>(T @event) where T : class
    {
        using var channel = _connection.CreateModel();
        var queueName = typeof(T).Name;

        channel.QueueDeclare(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var json = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);

        return Task.CompletedTask;
    }
}
