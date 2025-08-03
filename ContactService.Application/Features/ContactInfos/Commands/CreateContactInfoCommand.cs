using MediatR;

namespace ContactService.Application.Features.ContactInfo.Commands;

public class CreateContactInfoCommand : IRequest<Guid>
{
    public Guid PersonId { get; set; }
    public string InfoType { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}
