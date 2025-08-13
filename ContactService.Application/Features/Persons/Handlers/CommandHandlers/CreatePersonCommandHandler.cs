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
    private readonly IGenericRepository<Person> _personRepository;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    public const string PersonCreated = "person-created-queue";

    public CreatePersonCommandHandler(IGenericRepository<Person> personRepository, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _personRepository = personRepository;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = _mapper.Map<Person>(request);
        person.Id = Guid.NewGuid();

        await _personRepository.AddAsync(person);
        await _personRepository.SaveChangesAsync();

        var @event = new PersonCreatedEvent(
            person.Id,
            $"{person.FirstName} {person.LastName}",
            DateTime.UtcNow
        );


        await _publishEndpoint.Publish(@event);

        return person.Id;
    }
}
