using ContactService.Application.Interfaces;
using MassTransit;
using SharedKernel.Enums;
using SharedKernel.Events.Reports;
using SharedKernel.Interfaces;

namespace ContactService.Application.Features.Reports.Consumers;

public class ReportRequestedEventConsumer : IConsumer<ReportRequestedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IContactInfoService _contactInfoService;
    private readonly IAppLogger<ReportRequestedEventConsumer> _logger;

    public ReportRequestedEventConsumer(IPublishEndpoint publishEndpoint,
        IContactInfoService contactInfoService, IAppLogger<ReportRequestedEventConsumer> logger)
    {
        _publishEndpoint = publishEndpoint;
        _contactInfoService = contactInfoService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ReportRequestedEvent> context)
    {
        var message = context.Message;

        // CorrelationId header’ı al
        var correlationId = context.Headers.TryGetHeader("X-Correlation-ID", out var value)
            ? value?.ToString()
            : Guid.NewGuid().ToString();

        _logger.LogInformation($"ReportRequestedEvent received with CorrelationId: {correlationId}");
    //    _logger.LogInformation("ReportRequestedEvent received. CorrelationId: {CorrelationId}, Location: {Location}",
    //correlationId, message.Location);

        // Lokasyona göre Contact bilgilerini çek
        var contacts = await _contactInfoService.GetContactsByLocationAsync(message.Location);

        _logger.LogInformation($"Found {contacts.Count} contacts for Location: {context.Message.Location}");

        var contactDtos = contacts.Select(c => new ContactDto
        {
            ContactId = c.Id,
            FullName = c.Person != null ? $"{c.Person.FirstName} {c.Person.LastName}" : string.Empty,
            Email = c.Person?.ContactInfos?.FirstOrDefault(ci => ci.Type == ContactType.Email)?.Value,
            Phone = c.Person?.ContactInfos?.FirstOrDefault(ci => ci.Type == ContactType.Phone)?.Value
        }).ToList();

        // Rapor için hazırlanmış event
        var responseEvent = new ReportContactsPreparedEvent
        {
            ReportId = message.ReportId,
            Location = message.Location,
            Contacts = contactDtos
        };

        await _publishEndpoint.Publish(responseEvent, ctx =>
        {
            ctx.Headers.Set("X-Correlation-ID", correlationId);
        });

        _logger.LogInformation($"[CorrelationId: {correlationId}] ReportContactsPreparedEvent published for ReportId {message.ReportId}");

    //    _logger.LogInformation("ReportContactsPreparedEvent published. CorrelationId: {CorrelationId}, Contacts: {Count}",
    //correlationId, contactDtos.Count);
    }
}
