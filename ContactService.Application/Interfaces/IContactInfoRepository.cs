using ContactService.Domain.Entities;

namespace ContactService.Application.Interfaces;

public interface IContactInfoRepository : IGenericRepository<ContactInfo>
{
    Task<List<ContactInfo>> GetContactsByLocationAsync(string location);
}
