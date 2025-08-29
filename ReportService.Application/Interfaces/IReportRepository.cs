using ReportService.Domain.Entities;
using SharedKernel.Events.Reports;

namespace ReportService.Application.Interfaces
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task UpdateReportContactsAsync(Guid reportId, List<ContactDto> contacts, string location);
    }
}
