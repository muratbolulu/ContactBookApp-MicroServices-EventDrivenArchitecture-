using AutoMapper;
using MediatR;
using ReportService.Application.Features.Reports.Commands;
using ReportService.Domain.Entities;
using ReportService.Domain.Enums;
using ReportService.Domain.Interfaces;
using SharedKernel.Interface;

namespace ReportService.Application.Features.Reports.Handlers.Commands;
public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand, Guid>
{
    private readonly IReportRepository _repository;
    private readonly IMapper _mapper;

    public CreateReportCommandHandler(IReportRepository repository, IMapper mapper)
    {
        _repository = repository;
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

        await _repository.AddAsync(report);
        await _repository.SaveChangesAsync();

        return report.Id;
    }
}
