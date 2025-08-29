using ContactService.Application.DTOs;
using MediatR;

namespace ContactService.Application.Features.Persons.Queries;

public class GetAllPersonsQuery : IRequest<List<PersonDetailDto>>
{
}
