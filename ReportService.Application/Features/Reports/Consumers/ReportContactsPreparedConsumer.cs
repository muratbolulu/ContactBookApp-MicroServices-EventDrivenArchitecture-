using MassTransit;
using ReportService.Application.Interfaces;
using SharedKernel.Events.Reports;
using SharedKernel.Interfaces;

namespace ReportService.Application.Features.Reports.Consumers;

public class ReportContactsPreparedConsumer : IConsumer<ReportContactsPreparedEvent>
{
    private readonly IReportRepository _reportRepository;
    private readonly IAppLogger<ReportContactsPreparedConsumer> _logger;

    public ReportContactsPreparedConsumer(IReportRepository reportRepository, IAppLogger<ReportContactsPreparedConsumer> logger)
    {
        _reportRepository = reportRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ReportContactsPreparedEvent> context)
    {
        var message = context.Message;

        // CorrelationId’yi header’dan oku
        var correlationId = context.Headers.Get<string>("X-Correlation-ID") ?? Guid.NewGuid().ToString();

        _logger.LogInformation("ReportContactsPreparedEvent received. CorrelationId: {CorrelationId}",
            correlationId);

        // Raporu update et (Contacts JSON olarak Content’e yazılacak)
        // Rapor bulunamadıysa işlem yapma 'UpdateReportContactsAsync' metodu içinde handle edildi.
        await _reportRepository.UpdateReportContactsAsync(
            message.ReportId,
            message.Contacts,
            message.Location
        );

        _logger.LogInformation("Report updated successfully. CorrelationId: {CorrelationId}", correlationId);
    }
}
