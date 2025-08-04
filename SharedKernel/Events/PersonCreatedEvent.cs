namespace SharedKernel.Events;

public class PersonCreatedEvent
{
    public Guid PersonId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

