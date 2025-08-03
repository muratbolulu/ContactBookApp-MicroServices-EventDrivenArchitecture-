using AutoMapper;
using ContactService.Application.Features.Persons.Commands;
using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using ContactService.Domain.Interfaces;
using MediatR;

namespace ContactService.Application.Features.Persons.Handlers.CommandHandlers;

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, Guid>
{
    private readonly IGenericRepository<Person> _personRepository;
    private readonly IMapper _mapper;

    public CreatePersonCommandHandler(IGenericRepository<Person> personRepository, IMapper mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = _mapper.Map<Person>(request);
        person.Id = Guid.NewGuid();

        await _personRepository.AddAsync(person);
        return person.Id;
    }
}
