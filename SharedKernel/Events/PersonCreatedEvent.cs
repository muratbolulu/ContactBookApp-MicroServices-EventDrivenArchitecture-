namespace SharedKernel.Events;

public class PersonCreatedEvent
{
    public Guid PersonId { get; set; }
    public string FullName { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}

