using MediatR;

namespace ContactService.Application.Features.ContactInfos.Commands;

public class DeleteContactInfoCommand : IRequest<bool>
{
    public Guid Id { get; set; }

    public DeleteContactInfoCommand(Guid id)
    {
        Id = id;
    }
}
