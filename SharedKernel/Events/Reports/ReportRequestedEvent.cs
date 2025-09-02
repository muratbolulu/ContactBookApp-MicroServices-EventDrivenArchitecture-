namespace SharedKernel.Events.Reports;

public class ReportRequestedEvent
{
    public Guid ReportId { get; set; }
    public string Location { get; set; } = null!;  // Lokasyon bilgisi belki sonra LocationId eklenebilir.
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
}
