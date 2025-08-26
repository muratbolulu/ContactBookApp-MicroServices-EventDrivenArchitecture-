using AutoMapper;
using ContactService.Application.Features.Persons.Commands;
using ContactService.Domain.Entities;
using MassTransit;
using MediatR;
using SharedKernel.Events;
using SharedKernel.Interface;

namespace ContactService.Application.Features.Persons.Handlers.CommandHandlers;

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, Guid>
{
    private readonly IGenericService<Person> _personService;
    private readonly IMapper _mapper;

    public CreatePersonCommandHandler(IGenericService<Person> personService, IMapper mapper)
    {
        _personService = personService;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = _mapper.Map<Person>(request);
        person.Id = Guid.NewGuid();

        await _personService.AddAsync(person);
        await _personService.SaveChangesAsync();

        return person.Id;
    }
}
