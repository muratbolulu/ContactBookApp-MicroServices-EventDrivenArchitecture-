namespace SharedKernel.Messaging.Abstraction;

public interface IRabbitMQProducer
{
    void Publish<T>(T message, string queueName);
}
