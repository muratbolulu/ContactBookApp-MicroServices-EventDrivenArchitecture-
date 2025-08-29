using Microsoft.EntityFrameworkCore;
using ReportService.Application.Interfaces;
using ReportService.Domain.Entities;
using SharedKernel.Events.Reports;

namespace ReportService.Infrastructure.Persistence.Repositories
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        private readonly ReportDbContext _context;

        public ReportRepository(ReportDbContext context):base(context)
        {
            _context = context;
        }

        public async Task UpdateReportContactsAsync(Guid reportId, List<ContactDto> contacts, string location)
        {
            var report = await _context.Reports
                .Include(r => r.Contacts)
                .FirstOrDefaultAsync(r => r.Id == reportId);

            if (report == null)
            {
                // Yeni rapor oluştur
                report = new Report
                {
                    Id = reportId,
                    Location = location,
                };
                _context.Reports.Add(report);
            }

            // Eski contact’ları temizle
            report.Contacts.Clear();

            // Yeni contact’ları ekle
            report.Contacts.AddRange(contacts.Select(c => new ReportContact
            {
                ContactId = c.ContactId,
                FullName = c.FullName,
                Email = c.Email,
                Phone = c.Phone,
                ReportId = report.Id
            }));

            await _context.SaveChangesAsync();
        }
    }
}
