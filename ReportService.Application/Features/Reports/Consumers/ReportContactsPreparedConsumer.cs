using MassTransit;
using ReportService.Application.Interfaces;
using SharedKernel.Events.Reports;

namespace ReportService.Application.Features.Reports.Consumers;

public class ReportContactsPreparedConsumer : IConsumer<ReportContactsPreparedEvent>
{
    private readonly IReportRepository _reportRepository;

    public ReportContactsPreparedConsumer(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task Consume(ConsumeContext<ReportContactsPreparedEvent> context)
    {
        var message = context.Message;

        var report = await _reportRepository.GetByIdAsync(message.ReportId);
        if (report == null) return;

        await _reportRepository.UpdateReportContactsAsync(
            message.ReportId,
            message.Contacts,
            message.Location
        );

        await _reportRepository.UpdateAsync(report);
    }
}
