using ContactService.Application.Interfaces;
using ContactService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;

namespace ContactService.Infrastructure.Persistence.Repositories;

public class ContactInfoRepository : GenericRepository<ContactInfo>, IContactInfoRepository
{
    private readonly ContactDbContext _context;

    public ContactInfoRepository(ContactDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<ContactInfo>> GetContactsByLocationAsync(string location)
    {
        // 1. Location bilgisi ile kişileri bul
        var personsInLocation = await _context.ContactInfos
            .Where(c => c.Type == ContactType.Location && c.Value == location)
            .Select(c => c.PersonId)
            .ToListAsync();

        // 2. Bu kişilerin telefon bilgilerini al
        var phones = await _context.ContactInfos
            .Where(c => c.Type == ContactType.Phone && personsInLocation.Contains(c.PersonId))
            .Include(c => c.Person) // istersen kişi bilgileri de gelsin
            .ToListAsync();

        return phones;
    }
}
