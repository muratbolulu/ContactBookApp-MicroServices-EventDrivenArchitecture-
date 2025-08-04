using MediatR;
using ReportService.Application.DTOs;

namespace ReportService.Application.Features.Reports.Queries;

public class GetReportByIdQuery : IRequest<ReportDto>
{
    public Guid Id { get; set; }

    public GetReportByIdQuery(Guid id)
    {
        Id = id;
    }
}
