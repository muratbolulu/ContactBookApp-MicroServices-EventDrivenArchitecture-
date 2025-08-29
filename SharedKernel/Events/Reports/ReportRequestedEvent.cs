namespace SharedKernel.Events.Reports;

public class ReportRequestedEvent
{
    public Guid ReportId { get; set; }       // Raporun unique ID'si
    public string Location { get; set; }     // Lokasyon bilgisi belki sonra LocationId eklenebilir.
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
}
