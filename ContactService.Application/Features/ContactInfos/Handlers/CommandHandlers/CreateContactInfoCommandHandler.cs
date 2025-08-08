using AutoMapper;
using ContactService.Application.Features.ContactInfo.Commands;
using ContactService.Domain.Entities;
using MediatR;
using SharedKernel.Interface;
using DomainContactInfo = ContactService.Domain.Entities.ContactInfo;


namespace ContactService.Application.Features.ContactInfos.Handlers.CommandHandlers;

public class CreateContactInfoCommandHandler : IRequestHandler<CreateContactInfoCommand, Guid>
{
    private readonly IGenericRepository<DomainContactInfo> _contactInfoRepository;
    private readonly IGenericRepository<Person> _personRepository;
    private readonly IMapper _mapper;

    public CreateContactInfoCommandHandler(IGenericRepository<DomainContactInfo> contactInfoRepository, IGenericRepository<Person> personRepository, IMapper mapper)
    {
        _contactInfoRepository = contactInfoRepository;
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateContactInfoCommand request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.PersonId);
        if (person == null)
            throw new KeyNotFoundException("Person not found.");

        var contactInfo = _mapper.Map<DomainContactInfo>(request);

        contactInfo.Id = Guid.NewGuid(); // Eğer DB kendisi ID üretmiyorsa

        await _contactInfoRepository.AddAsync(contactInfo);
        await _contactInfoRepository.SaveChangesAsync();

        return contactInfo.Id;
    }
}
