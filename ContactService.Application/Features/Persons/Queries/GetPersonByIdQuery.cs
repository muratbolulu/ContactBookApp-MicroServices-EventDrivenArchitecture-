using MediatR;
using ContactService.Application.DTOs;

namespace ContactService.Application.Features.Persons.Queries;

public class GetPersonByIdQuery : IRequest<PersonDetailDto>
{
    public Guid Id { get; set; }

    public GetPersonByIdQuery(Guid id)
    {
        Id = id;
    }
}
