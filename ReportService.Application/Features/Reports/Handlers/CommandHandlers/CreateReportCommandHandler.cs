using AutoMapper;
using MassTransit;
using MediatR;
using ReportService.Application.Features.Reports.Commands;
using ReportService.Domain.Entities;
using ReportService.Domain.Enums;
using SharedKernel.Events.Reports;
using SharedKernel.Interface;

namespace ReportService.Application.Features.Reports.Handlers.CommandHandlers;
public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Guid>
{
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateReportCommandHandler(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var reportId = Guid.NewGuid();

        var evt = new ReportRequestedEvent
        {
            ReportId = reportId,
            Location = request.Location,
            RequestedAt = DateTime.UtcNow
        };

        await _publishEndpoint.Publish(evt, cancellationToken);

        return reportId;
    }
}
