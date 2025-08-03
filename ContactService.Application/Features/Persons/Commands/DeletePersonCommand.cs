using MediatR;

namespace ContactService.Application.Features.Persons.Commands;

public class DeletePersonCommand : IRequest<bool>
{
    public Guid Id { get; set; }

    public DeletePersonCommand(Guid id)
    {
        Id = id;
    }
}
