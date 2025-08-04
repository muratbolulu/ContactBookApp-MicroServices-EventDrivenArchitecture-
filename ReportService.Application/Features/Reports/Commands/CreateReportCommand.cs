using MediatR;

namespace ReportService.Application.Features.Reports.Commands;

public class CreateReportCommand : IRequest<Guid> 
{
    public string Location { get; set; } = string.Empty;
}
