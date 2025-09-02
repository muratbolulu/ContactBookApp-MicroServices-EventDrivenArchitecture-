using ContactService.Application.Interfaces;
using MassTransit;
using SharedKernel.Enums;
using SharedKernel.Events.Reports;

namespace ContactService.Application.Features.Reports.Consumers;

public class ReportRequestedEventConsumer : IConsumer<ReportRequestedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IContactInfoService _contactInfoService;

    public ReportRequestedEventConsumer(
        IPublishEndpoint publishEndpoint,
        IContactInfoService contactInfoService)
    {
        _publishEndpoint = publishEndpoint;
        _contactInfoService = contactInfoService;
    }

    public async Task Consume(ConsumeContext<ReportRequestedEvent> context)
    {
        var message = context.Message;

        // Lokasyona göre Contact bilgilerini çek
        var contacts = await _contactInfoService.GetContactsByLocationAsync(message.Location);

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

        await _publishEndpoint.Publish(responseEvent);
    }
}
