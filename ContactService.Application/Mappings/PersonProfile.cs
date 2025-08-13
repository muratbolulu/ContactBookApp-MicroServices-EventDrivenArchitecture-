using AutoMapper;
using ContactService.Application.DTOs;
using ContactService.Application.Features.Persons.Commands;
using ContactService.Domain.Entities;

namespace ContactService.Application.Mappings;

public class PersonProfile : Profile
{
    public PersonProfile()
    {
        CreateMap<Person, PersonDetailDto>();
        CreateMap<CreatePersonCommand, Person>();
        //CreateMap<PersonDetailDto, Person>();
        //CreateMap<ContactInfo, ContactInfoDto>();

        //CreateMap<CreatePersonCommand, Person>();
    }
}
