using ContactService.Application.DTOs;
using MediatR;

namespace ContactService.Application.Features.ContactInfos.Queries;

public class GetContactInfoByIdQuery : IRequest<ContactInfoDto>
{
    public Guid Id { get; set; }

    public GetContactInfoByIdQuery(Guid id)
    {
        Id = id;
    }
}
