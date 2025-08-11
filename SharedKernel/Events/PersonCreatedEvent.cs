namespace SharedKernel.Events;

public record PersonCreatedEvent(Guid PersonId, string FullName, DateTime CreatedAt);


