using Microsoft.EntityFrameworkCore;
using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;
using SharedKernel.Events.Reports;
using System.Text.Json;

namespace ReportService.Infrastructure.Persistence.Repositories;

public class ReportRepository : GenericRepository<Report>, IReportRepository
{
    private readonly ReportDbContext _context;

    public ReportRepository(ReportDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task UpdateReportContactsAsync(Guid reportId, List<ContactDto> contacts, string location)
    {
        // Raporu DB'den çek
        var report = await _context.Reports.FirstOrDefaultAsync(r => r.Id == reportId);
        if (report == null)
            throw new InvalidOperationException($"Report with ID {reportId} not found.");

        // Contacts listesini JSON olarak Content alanına yaz
        var json = JsonSerializer.Serialize(new
        {
            Location = location,
            Contacts = contacts
        });

        report.Content = json;
        report.Status = Domain.Enums.ReportStatus.Completed;
        report.CompletedAt = DateTime.UtcNow;

        _context.Reports.Update(report);
        await _context.SaveChangesAsync();
    }
}
