using AutoMapper;
using ContactService.Application.DTOs;
using ContactService.Application.Features.Persons.Queries;
using ContactService.Domain.Entities;
using MediatR;
using SharedKernel.Interface;

namespace ContactService.Application.Features.Persons.Handlers.QueryHandlers;

public class GetAllPersonsQueryHandler : IRequestHandler<GetAllPersonsQuery, List<PersonDetailDto>>
{
    private readonly IGenericRepository<Person> _personRepository;
    private readonly IMapper _mapper;

    public GetAllPersonsQueryHandler(IGenericRepository<Person> personRepository, IMapper mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    public async Task<List<PersonDetailDto>> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
    {
        var persons = await _personRepository.GetAllAsync();
        return _mapper.Map<List<PersonDetailDto>>(persons);
    }
}
