using ReportService.Domain.Enums;

namespace ReportService.Domain.Entities;

public class Report
{
    public Guid Id { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public string Location { get; set; } // tekrar dönülecek
    public ReportStatus Status { get; set; } = ReportStatus.Pending;
    public string FilePath { get; set; }
    public string? Content { get; set; }
}
