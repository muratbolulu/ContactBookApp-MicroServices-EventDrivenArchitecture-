using AutoMapper;
using ContactService.Application.Features.ContactInfo.Commands;
using ContactService.Domain.Entities;

namespace ContactService.Application.Mappings
{
    public class ContactInfoProfile : Profile
    {
        public ContactInfoProfile()
        {
            CreateMap<CreateContactInfoCommand, ContactInfo>();
        }
    }
}
