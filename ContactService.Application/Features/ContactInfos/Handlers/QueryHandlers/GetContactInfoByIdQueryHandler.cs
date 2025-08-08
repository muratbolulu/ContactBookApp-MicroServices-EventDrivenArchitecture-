using AutoMapper;
using ContactService.Application.DTOs;
using ContactService.Application.Features.ContactInfos.Queries;
using MediatR;
using SharedKernel.Interface;

namespace ContactService.Application.Features.ContactInfos.Handlers.QueryHandlers;

public class GetContactInfoByIdQueryHandler : IRequestHandler<GetContactInfoByIdQuery, ContactInfoDto>
{
    private readonly IGenericRepository<Domain.Entities.ContactInfo> _contactInfoRepository;
    private readonly IMapper _mapper;

    public GetContactInfoByIdQueryHandler(IGenericRepository<Domain.Entities.ContactInfo> contactInfoRepository, IMapper mapper)
    {
        _contactInfoRepository = contactInfoRepository;
        _mapper = mapper;
    }

    public async Task<ContactInfoDto> Handle(GetContactInfoByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _contactInfoRepository.GetByIdAsync(request.Id);
        if (entity == null) return null;

        return _mapper.Map<ContactInfoDto>(entity);

    }

}
