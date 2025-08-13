using MassTransit;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Persistence;
using SharedKernel.Events;

namespace ReportService.Infrastructure.EventHandlers;

public class PersonCreatedEventConsumer : IConsumer<PersonCreatedEvent>
{
    private readonly ReportDb _dbContext;

    public PersonCreatedEventConsumer(ReportDb dbContext)
    {
        _dbContext = dbContext;
    }

    public PersonCreatedEventConsumer()
    {
    }

    public async Task Consume(ConsumeContext<PersonCreatedEvent> context)
    {
        var message = context.Message;

        var report = new Report
        {
            Id = Guid.NewGuid(),
            RequestedAt = DateTime.UtcNow,
            Status = Domain.Enums.ReportStatus.Pending
        };

        _dbContext.Reports.Add(report);
        await _dbContext.SaveChangesAsync();

        Console.WriteLine($"Kişi oluşturuldu: {message.PersonId} - {message.FullName}");
    }
}
