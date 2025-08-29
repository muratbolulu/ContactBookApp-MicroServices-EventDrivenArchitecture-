using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Persistence;
using ReportService.Infrastructure.Persistence.Repositories;
using SharedKernel.Events.Reports;

namespace ReportService.Infrastructure.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    public ReportRepository(ReportDbContext context) : base(context)
    {
    }

    public Task UpdateReportContactsAsync(Guid reportId, List<ContactDto> contacts, string location)
    {
        throw new NotImplementedException();
    }
}
