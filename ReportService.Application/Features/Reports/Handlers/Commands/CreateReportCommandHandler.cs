using ReportService.Application.Features.Reports.Commands;
using ReportService.Domain.Entities;

namespace ReportService.Application.Features.Reports.Handlers.Commands;

public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Guid>
{
    private readonly IGenericRepository<Report> _repository;

    public CreateReportCommandHandler(IGenericRepository<Report> repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var report = new Report
        {
            Id = Guid.NewGuid(),
            Status = "Preparing",
            RequestedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(report);
        await _repository.SaveChangesAsync();

        return report.Id;
    }
}
