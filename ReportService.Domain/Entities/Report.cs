using ReportService.Domain.Enums;

namespace ReportService.Domain.Entities;

public class Report
{
    public Guid Id { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public string Location { get; set; } = null!;
    public ReportStatus Status { get; set; } = ReportStatus.Pending;
    public string? Content { get; set; }

    public List<ReportContact> Contacts { get; set; } = new();

}
