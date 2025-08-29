using SharedKernel.Events.Reports;

namespace ContactService.Infrastructure.Messaging.Services;

public class ReportService : IReportService
{
    public Task HandleReportCreatedAsync(int reportId, string title, DateTime createdAt)
    {
        // Rapor oluşturma sonrası yapılacak işlemler
        Console.WriteLine($"Report received: {reportId}, {title}");
        return Task.CompletedTask;
    }

    public Task UpdateReportContactsAsync(Guid reportId, List<ContactDto> contacts, string location)
    {
        //burası doldurulacak
        throw new NotImplementedException();
    }
}
