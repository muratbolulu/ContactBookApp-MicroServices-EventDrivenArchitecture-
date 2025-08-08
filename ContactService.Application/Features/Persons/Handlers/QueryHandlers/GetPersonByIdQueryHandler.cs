using AutoMapper;
using ContactService.Application.DTOs;
using ContactService.Application.Features.Persons.Queries;
using ContactService.Domain.Entities;
using MediatR;
using SharedKernel.Interface;

namespace ContactService.Application.Features.Persons.Handlers.QueryHandlers;

public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonDetailDto>
{
    private readonly IGenericRepository<Person> _personRepository;
    private readonly IMapper _mapper;

    public GetPersonByIdQueryHandler(IGenericRepository<Person> personRepository, IMapper mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public async Task<PersonDetailDto?> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var person = await _personRepository.GetByIdAsync(request.Id);
        if (person == null)
            return null;

        return _mapper.Map<PersonDetailDto>(person);
    }
}
