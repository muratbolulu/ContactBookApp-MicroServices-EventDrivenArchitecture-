using ContactService.Application.Interfaces;
using ContactService.Application.Services;
using ContactService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Infrastructure.Persistence.Repositories;

public class ContactInfoRepository : GenericRepository<ContactInfo>, IContactInfoRepository
{
    public ContactInfoRepository(ContactDbContext context) : base(context)
    {
    }

    public Task<List<ContactInfo>> GetContactsByLocationAsync(string location)
    {
        throw new NotImplementedException();
    }
}
