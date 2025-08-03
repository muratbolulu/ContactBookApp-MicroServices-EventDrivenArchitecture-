using ContactService.Application.DTOs;
using MediatR;
using System.Collections.Generic;

namespace ContactService.Application.Features.Persons.Queries;

public class GetAllPersonsQuery : IRequest<List<PersonDetailDto>>
{
}
