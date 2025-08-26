using ContactService.Application.Features.Persons.Commands;
using ContactService.Application.Interfaces;
using MediatR;

namespace ContactService.Application.Features.Persons.Handlers.CommandHandlers;

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, bool>
{
    private readonly IPersonService _personService;

    public DeletePersonCommandHandler(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<bool> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personService.GetByIdAsync(request.Id);
        if (person == null)
            return false;

        await _personService.DeleteAsync(person);
        await _personService.SaveChangesAsync();

        return true;
    }
}
