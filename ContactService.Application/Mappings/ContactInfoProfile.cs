using AutoMapper;
using ContactService.Application.Features.ContactInfo.Commands;
using ContactService.Domain.Entities;

namespace ContactService.Application.Mappings;

public class ContactInfoProfile : Profile
{
    public ContactInfoProfile()
    {
        CreateMap<CreateContactInfoCommand, ContactInfo>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.InfoType)); // enum -> enum

    }
}