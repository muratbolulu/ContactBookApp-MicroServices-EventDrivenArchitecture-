using AutoMapper;
using ContactService.Application.Features.ContactInfo.Commands;
using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using MediatR;
using SharedKernel.Interface;
using DomainContactInfo = ContactService.Domain.Entities.ContactInfo;


namespace ContactService.Application.Features.ContactInfos.Handlers.CommandHandlers;

public class CreateContactInfoCommandHandler : IRequestHandler<CreateContactInfoCommand, Guid>
{
    private readonly IContactInfoService _contactInfoService;
    private readonly IPersonService _personService;
    private readonly IMapper _mapper;

    public CreateContactInfoCommandHandler(IContactInfoService contactInfoService, IPersonService personService, IMapper mapper)
    {
        _contactInfoService = contactInfoService;
        _personService = personService;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateContactInfoCommand request, CancellationToken cancellationToken)
    {
        var person = await _personService.GetByIdAsync(request.PersonId);
        if (person == null)
            throw new KeyNotFoundException("Person not found.");

        var contactInfo = _mapper.Map<DomainContactInfo>(request);

        contactInfo.Id = Guid.NewGuid(); // Eğer DB kendisi ID üretmiyorsa

        await _contactInfoService.AddAsync(contactInfo);
        await _contactInfoService.SaveChangesAsync();

        return contactInfo.Id;
    }
}
