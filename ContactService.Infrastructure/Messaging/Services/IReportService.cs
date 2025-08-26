using SharedKernel.Events.Reports;

namespace ContactService.Infrastructure.Messaging.Services;

public interface IReportService
{
    Task HandleReportCreatedAsync(int reportId, string title, DateTime createdAt);

    Task UpdateReportContactsAsync(Guid reportId, List<ContactDto> contacts, string location);

}