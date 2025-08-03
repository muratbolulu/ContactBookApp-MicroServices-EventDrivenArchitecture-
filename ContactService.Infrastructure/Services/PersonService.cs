using ContactService.Application.DTOs;
using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using ContactService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Infrastructure.Services;

public class PersonService : IPersonService
{
    private readonly ProjectDbContext _context;

    public PersonService(ProjectDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreatePersonAsync(CreatePersonDto dto)
    {
        var person = new Person
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Company = dto.Company
        };

        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();

        return person.Id;
    }

    public async Task DeletePersonAsync(Guid id)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return;

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
    }

    public async Task<List<PersonDetailDto>> GetAllPersonsAsync()
    {
        var persons = await _context.Persons
            .Include(p => p.ContactInfos)
            .ToListAsync();

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
        var p = await _context.Persons
            .Include(x => x.ContactInfos)
            .FirstOrDefaultAsync(x => x.Id == id);

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
