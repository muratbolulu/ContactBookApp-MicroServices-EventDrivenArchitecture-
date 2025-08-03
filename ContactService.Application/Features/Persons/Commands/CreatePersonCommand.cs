using MediatR;

namespace ContactService.Application.Features.Persons.Commands;

public class CreatePersonCommand : IRequest<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Company { get; set; } = default!;
}
