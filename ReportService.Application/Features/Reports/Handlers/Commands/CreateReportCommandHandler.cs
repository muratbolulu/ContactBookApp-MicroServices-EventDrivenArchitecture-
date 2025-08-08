using AutoMapper;
using MediatR;
using ReportService.Application.Features.Reports.Commands;
using ReportService.Domain.Entities;
using ReportService.Domain.Enums;
using SharedKernel.Interface;

namespace ReportService.Application.Features.Reports.Handlers.Commands;
public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Guid>
{
    private readonly IGenericRepository<Report> _reportRepository;
    private readonly IMapper _mapper;

    public CreateReportCommandHandler(IGenericRepository<Report> reportRepository, IMapper mapper)
    {
        _reportRepository = reportRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var report = new Report
        {
            Id = Guid.NewGuid(),
            Location = request.Location,
            RequestedAt = DateTime.UtcNow,
            Status = ReportStatus.Pending
        };

        await _reportRepository.AddAsync(report);
        await _reportRepository.SaveChangesAsync();

        return report.Id;
    }
}
