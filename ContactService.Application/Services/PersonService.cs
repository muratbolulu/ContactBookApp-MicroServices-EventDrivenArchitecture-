using ContactService.Application.DTOs;
using ContactService.Application.Interfaces;
using ContactService.Application.Services;
using ContactService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Application.Services;

public class PersonService : GenericService<Person>, IPersonService
{
    private readonly IGenericRepository<Person> _personRepository;

    public PersonService(IGenericRepository<Person> personRepository) : base(personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<Guid> CreatePersonAsync(CreatePersonDto dto)
    {
        var person = new Person
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Company = dto.Company
        };

        await _personRepository.AddAsync(person);
        await _personRepository.SaveChangesAsync();

        return person.Id;
    }

    public async Task DeletePersonAsync(Guid id)
    {
        var person = await _personRepository.GetByIdAsync(id);
        if (person == null) return;

        await _personRepository.DeleteAsync(person);
        await _personRepository.SaveChangesAsync();
    }

    public async Task<List<PersonDetailDto>> GetAllPersonsAsync()
    {
        var persons = await _personRepository
                .GetWhereAsync(predicate: null,
                               include: q => q.Include(p => p.ContactInfos));

        // yapıyı handle ettikten sonra, burayı sorgu içine model göndererek alacağım.
        // şu an data çekiliyor sonra mapleniyor. :(
        return persons.Select(p => new PersonDetailDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Company = p.Company,
            ContactInfos = p.ContactInfos.Select(ci => new ContactInfoDto
            {
                Id = ci.Id,
                Type = ci.Type.ToString(),
                Value = ci.Value
            }).ToList()
        }).ToList();
    }

    public async Task<PersonDetailDto> GetPersonWithContactsAsync(Guid id)
    {
        var p = await _personRepository.GetByIdAsync(
            predicate: x => x.Id == id,
            include: q => q.Include(p => p.ContactInfos)
        );

        if (p == null) return null!;

        return new PersonDetailDto
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Company = p.Company,
            ContactInfos = p.ContactInfos.Select(ci => new ContactInfoDto
            {
                Id = ci.Id,
                Type = ci.Type.ToString(),
                Value = ci.Value
            }).ToList()
        };
    }
}
