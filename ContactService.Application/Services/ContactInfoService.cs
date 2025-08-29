using ContactService.Application.DTOs;
using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;

namespace ContactService.Application.Services;

public class ContactInfoService : GenericService<ContactInfo>, IContactInfoService
{
    private readonly IContactInfoRepository _contactInfoRepository;

    public ContactInfoService(IContactInfoRepository contactInfoRepository) : base(contactInfoRepository)
    {
        _contactInfoRepository = contactInfoRepository;
    }

    public async Task AddContactInfoAsync(AddContactInfoDto dto)
    {
        var personExists = await _contactInfoRepository.AnyAsync(p => p.Id == dto.PersonId);

        if (!personExists)
            throw new ArgumentException("Person not found");

        var contactInfo = new ContactInfo
        {
            PersonId = dto.PersonId,
            Type = dto.Type,
            Value = dto.Value
        };

        await _contactInfoRepository.AddAsync(contactInfo);
        await _contactInfoRepository.SaveChangesAsync();
    }

    public async Task RemoveContactInfoAsync(Guid contactInfoId)
    {
        var contactInfo = await _contactInfoRepository.GetByIdAsync(contactInfoId);
        if (contactInfo == null) return;

        await _contactInfoRepository.DeleteAsync(contactInfo);
        await _contactInfoRepository.SaveChangesAsync();
    }

    public async Task<List<ContactInfo>> GetContactsByLocationAsync(string location)
    {
        return await _contactInfoRepository.GetContactsByLocationAsync(location);
    }
}

