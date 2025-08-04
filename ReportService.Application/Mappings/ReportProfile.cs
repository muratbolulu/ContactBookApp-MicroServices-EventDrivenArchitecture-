using AutoMapper;
using ReportService.Application.Features.Reports.Commands;
using ReportService.Domain.Entities;

namespace ReportService.Application.Mappings
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<CreateReportCommand, Report>();
        }
    }
}
