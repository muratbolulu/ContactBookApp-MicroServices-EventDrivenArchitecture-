using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using ReportService.Application.Features.Reports.Commands;
using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;
using SharedKernel.Events.Reports;
using SharedKernel.Interfaces;

namespace ReportService.Application.Features.Reports.Handlers.CommandHandlers;
public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Guid>
{
    private readonly IReportRepository _reportRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAppLogger<CreateReportCommandHandler> _logger;

    public CreateReportCommandHandler(IReportRepository reportRepository, IPublishEndpoint publishEndpoint, 
        IHttpContextAccessor httpContextAccessor, IAppLogger<CreateReportCommandHandler> logger)
    {
        _reportRepository = reportRepository;
        _publishEndpoint = publishEndpoint;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var reportId = Guid.NewGuid();

        var correlationId = _httpContextAccessor.HttpContext?.Items["X-Correlation-ID"]?.ToString()
                           ?? Guid.NewGuid().ToString();

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

        _logger.LogInformation($"[CorrelationId: {correlationId}] Report created with Id: {report.Id}, Location: {report.Location}");

        // 2. ContactService’e "hazırla" isteği gönder
        var @event = new ReportRequestedEvent
        {
            ReportId = reportId,
            Location = request.Location,
            RequestedAt = DateTime.UtcNow
        };

        //queue durable olduğu için kalıcı (Program.cs te ayarlandı.)
        await _publishEndpoint.Publish(@event, ctx =>
        {
            ctx.Headers.Set("X-Correlation-ID", correlationId);
        }, cancellationToken);

        _logger.LogInformation($"[CorrelationId: {correlationId}] ReportRequestedEvent published for ReportId {report.Id}");


        return @event.ReportId;
    }
}
