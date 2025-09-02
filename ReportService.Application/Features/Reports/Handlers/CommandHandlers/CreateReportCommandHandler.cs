using MassTransit;
using MediatR;
using ReportService.Application.Features.Reports.Commands;
using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;
using SharedKernel.Events.Reports;

namespace ReportService.Application.Features.Reports.Handlers.CommandHandlers;
public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Guid>
{
    private readonly IReportRepository _reportRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateReportCommandHandler(IReportRepository reportRepository, IPublishEndpoint publishEndpoint)
    {
        _reportRepository = reportRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var reportId = Guid.NewGuid();

        // 1. DB’ye yeni rapor kaydı oluştur
        var report = new Report
        {
            Id = reportId,
            Location = request.Location,
            Status = Domain.Enums.ReportStatus.InProgress,
            RequestedAt = DateTime.UtcNow
        };

        await _reportRepository.AddAsync(report);
        await _reportRepository.SaveChangesAsync();

        // 2. ContactService’e "hazırla" isteği gönder
        var @event = new ReportRequestedEvent
        {
            ReportId = reportId,
            Location = request.Location,
            RequestedAt = DateTime.UtcNow
        };

        await _publishEndpoint.Publish(@event, cancellationToken);

        return @event.ReportId;
    }
}
