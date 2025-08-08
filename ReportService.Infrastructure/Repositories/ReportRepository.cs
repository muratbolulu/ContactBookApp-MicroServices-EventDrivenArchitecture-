using ReportService.Domain.Entities;
using ReportService.Infrastructure.Persistence;
using SharedKernel.Infrastructure;

namespace ReportService.Infrastructure.Repositories;

public class ReportRepository : GenericRepository<Report, ReportDbContext>
{
    public ReportRepository(ReportDbContext context) : base(context)
    {
    }
}
