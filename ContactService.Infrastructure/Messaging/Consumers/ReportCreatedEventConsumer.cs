using ContactService.Infrastructure.Messaging.Events;
using ContactService.Infrastructure.Messaging.Services;
using MassTransit;

namespace ContactService.Infrastructure.Messaging.Consumers;


public class ReportCreatedEventConsumer : IConsumer<ReportCreatedEvent>
{
    private readonly IReportService _reportService;

    public ReportCreatedEventConsumer(IReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task Consume(ConsumeContext<ReportCreatedEvent> context)
    {
        var message = context.Message;

        // Örnek: ReportService üzerinden bir işlem
        await _reportService.HandleReportCreatedAsync(message.ReportId, message.Title, message.CreatedAt);
    }
}
