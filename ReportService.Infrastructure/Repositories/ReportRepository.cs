using ReportService.Domain.Entities;
using ReportService.Domain.Interfaces;
using ReportService.Infrastructure.Persistence;
using SharedKernel.Infrastructure;

namespace ReportService.Infrastructure.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    public ReportRepository(ReportDbContext context) : base(context)
    {
    }
}
