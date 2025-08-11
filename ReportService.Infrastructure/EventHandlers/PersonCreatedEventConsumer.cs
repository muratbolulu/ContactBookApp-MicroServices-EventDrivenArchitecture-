using MassTransit;
using SharedKernel.Events;

namespace ReportService.Infrastructure.EventHandlers;

public class PersonCreatedEventConsumer : IConsumer<PersonCreatedEvent>
{
    public Task Consume(ConsumeContext<PersonCreatedEvent> context)
    {
        var message = context.Message;

        Console.WriteLine($"Kişi oluşturuldu: {message.PersonId} - {message.FullName}");

        return Task.CompletedTask;
    }

}
