namespace ReportService.Domain.Entities;

public class Report
{
    public Guid Id { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Preparing";
    public string FilePath { get; set; }
}
