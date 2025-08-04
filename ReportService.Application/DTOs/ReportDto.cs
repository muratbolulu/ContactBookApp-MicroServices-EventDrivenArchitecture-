namespace ReportService.Application.DTOs
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
    }
}
