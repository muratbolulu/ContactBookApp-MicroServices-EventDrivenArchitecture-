namespace ContactService.Infrastructure.Messaging.Events;

public record ReportCreatedEvent(int ReportId, string Title, DateTime CreatedAt);
