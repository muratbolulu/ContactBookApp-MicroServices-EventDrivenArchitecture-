namespace ReportService.API.Models;

public class CreateReportRequest
{
    // Raporun hazırlanacağı lokasyon
    public string Location { get; set; } = string.Empty;

    // public Guid RequestedByUserId { get; set; }
}
