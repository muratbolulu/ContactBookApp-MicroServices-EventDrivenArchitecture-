namespace SharedKernel.Events.Reports;

public class ReportContactsPreparedEvent
{
    public Guid ReportId { get; set; } // Hangi rapora ait olduğu
    public string Location { get; set; }
    public List<ContactDto> Contacts { get; set; } = new();
}