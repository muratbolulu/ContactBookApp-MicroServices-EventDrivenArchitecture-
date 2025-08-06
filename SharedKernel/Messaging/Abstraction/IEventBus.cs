namespace SharedKernel.Messaging.Abstraction;

public interface IEventBus
{
    Task PublishAsync<T>(T message, string queueName);
}