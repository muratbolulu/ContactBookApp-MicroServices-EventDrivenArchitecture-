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

        // Burada raporu güncelle veya veriyi DB'ye kaydet
        // Örneğin, Report entity'sine kişi listesini ekle
        await _reportRepository.UpdateReportContactsAsync(
            message.ReportId,
            message.Contacts,
            message.Location
        );

        // İstersen log yazabilir veya başka bir event tetikleyebilirsin
    }
}
