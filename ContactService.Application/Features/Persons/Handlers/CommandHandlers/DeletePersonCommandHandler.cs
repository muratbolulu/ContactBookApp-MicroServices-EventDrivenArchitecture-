using ContactService.Application.Features.Persons.Commands;
using ContactService.Domain.Entities;
using MediatR;
using SharedKernel.Interface;

namespace ContactService.Application.Features.Persons.Handlers.CommandHandlers;

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, bool>
{
    private readonly IGenericRepository<Person> _personRepository;

    public DeletePersonCommandHandler(IGenericRepository<Person> personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<bool> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id);
        if (person == null)
            return false;

        await _personRepository.DeleteAsync(person);
        await _personRepository.SaveChangesAsync();

        return true;
    }
}
