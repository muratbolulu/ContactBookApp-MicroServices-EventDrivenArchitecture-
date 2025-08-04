using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Text;

namespace ReportService.Application.EventHandlers;

public class PersonCreatedEventConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public PersonCreatedEventConsumer()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: "person_events", type: ExchangeType.Fanout);
        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: queueName, exchange: "person_events", routingKey: "");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var personCreatedEvent = JsonSerializer.Deserialize<PersonCreatedEvent>(message);

            if (personCreatedEvent != null)
            {
                //event alındığında yapılacak işlemler
                Console.WriteLine($"PersonCreatedEvent alındı: {personCreatedEvent.PersonId} - {personCreatedEvent.FullName} - {personCreatedEvent.CreatedAt}");
            }

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _channel.QueueDeclare().QueueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
