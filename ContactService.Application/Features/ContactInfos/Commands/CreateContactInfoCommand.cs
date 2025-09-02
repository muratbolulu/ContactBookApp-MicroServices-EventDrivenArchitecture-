using MediatR;
using SharedKernel.Enums;

namespace ContactService.Application.Features.ContactInfo.Commands;

public class CreateContactInfoCommand : IRequest<Guid>
{
    public Guid PersonId { get; set; }
    public ContactType InfoType { get; set; }
    public string Value { get; set; } = string.Empty;
}
