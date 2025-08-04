using AutoMapper;
using ContactService.Application.Features.Persons.Commands;
using ContactService.Domain.Entities;
using MediatR;
using SharedKernel.Events;
using SharedKernel.Interface;
using SharedKernel.Messaging;

namespace ContactService.Application.Features.Persons.Handlers.CommandHandlers;

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, Guid>
{
    private readonly IGenericRepository<Person> _personRepository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public CreatePersonCommandHandler(IGenericRepository<Person> personRepository, IMapper mapper, IEventBus eventBus)
    {
        _personRepository = personRepository;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    public async Task<Guid> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = _mapper.Map<Person>(request);
        person.Id = Guid.NewGuid();

        await _personRepository.AddAsync(person);
        await _personRepository.SaveChangesAsync();

        await _eventBus.PublishAsync(new PersonCreatedEvent
        {
            PersonId = person.Id,
            FullName = $"{person.FirstName} {person.LastName}",
            CreatedAt = DateTime.UtcNow
        });

        return person.Id;
    }
}
