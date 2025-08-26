using MediatR;

namespace ReportService.Application.Features.Reports.Commands;

public record CreateReportCommand(string Location) : IRequest<Guid>;