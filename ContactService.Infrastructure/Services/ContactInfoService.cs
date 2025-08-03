using ContactService.Application.DTOs;
using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using ContactService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Infrastructure.Services;

public class ContactInfoService : IContactInfoService
{
    private readonly ProjectDbContext _context;

    public ContactInfoService(ProjectDbContext context)
    {
        _context = context;
    }

    public async Task AddContactInfoAsync(AddContactInfoDto dto)
    {
        var personExists = await _context.Persons.AnyAsync(p => p.Id == dto.PersonId);
        if (!personExists)
            throw new ArgumentException("Person not found");

        var contactInfo = new ContactInfo
        {
            PersonId = dto.PersonId,
            Type = dto.Type,
            Value = dto.Value
        };

        await _context.ContactInfos.AddAsync(contactInfo);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveContactInfoAsync(Guid contactInfoId)
    {
        var contactInfo = await _context.ContactInfos.FindAsync(contactInfoId);
        if (contactInfo == null) return;

        _context.ContactInfos.Remove(contactInfo);
        await _context.SaveChangesAsync();
    }
}
