using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SharedKernel.Events;
using System.Text;
using System.Text.Json;

namespace ReportService.Infrastructure.Messaging
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQConsumerService()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: nameof(PersonCreatedEvent),
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var @event = JsonSerializer.Deserialize<PersonCreatedEvent>(message);

                if (@event != null)
                {
                    // TODO: Event'e karşılık yapılacak işlem
                    Console.WriteLine($"[x] Received PersonCreatedEvent: {@event.FullName} - {@event.PersonId}");

                }

                await Task.CompletedTask;
            };

            _channel.BasicConsume(queue: nameof(PersonCreatedEvent),
                                  autoAck: true,
                                  consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
