using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ReportService.Infrastructure.EventHandlers;

public class PersonCreatedEventConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;
    private readonly ILogger<PersonCreatedEventConsumer> _logger;

    public PersonCreatedEventConsumer(ILogger<PersonCreatedEventConsumer> logger)
    {
        _logger = logger;

        var factory = new ConnectionFactory() { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: "person_events", type: ExchangeType.Fanout);

        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: _queueName, exchange: "person_events", routingKey: "");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var personCreatedEvent = JsonSerializer.Deserialize<PersonCreatedEvent>(message);

                if (personCreatedEvent != null)
                {
                    _logger.LogInformation("PersonCreatedEvent alındı: {PersonId} - {FullName} - {CreatedAt}",
                        personCreatedEvent.PersonId,
                        personCreatedEvent.FullName,
                        personCreatedEvent.CreatedAt);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mesaj işlenirken hata oluştu.");
                // Hata durumunda BasicNack gönderilebilir
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
