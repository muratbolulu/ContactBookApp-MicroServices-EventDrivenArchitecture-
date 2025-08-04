using AutoMapper;
using MediatR;
using ReportService.Application.DTOs;
using ReportService.Application.Features.Reports.Queries;
using ReportService.Domain.Entities;
using SharedKernel.Interface;

namespace ReportService.Application.Features.Reports.Handlers.Queries;

public class GetReportByIdQueryHandler : IRequestHandler<GetReportByIdQuery, ReportDto>
{
    private readonly IGenericRepository<Report> _reportRepository;
    private readonly IMapper _mapper;

    public GetReportByIdQueryHandler(IGenericRepository<Report> reportRepository, IMapper mapper)
    {
        _reportRepository = reportRepository;
        _mapper = mapper;
    }

    public async Task<ReportDto> Handle(GetReportByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _reportRepository.GetByIdAsync(request.Id);
        if (entity == null)
            return null;

        return _mapper.Map<ReportDto>(entity);
    }
}
