using ContactService.Application.DTOs;
using ContactService.Application.Interface;
using ContactService.Domain.Entities;

namespace ContactService.Application.Interfaces;

public interface IContactInfoService : IGenericService<ContactInfo>
{
    Task AddContactInfoAsync(AddContactInfoDto dto);
    Task RemoveContactInfoAsync(Guid contactInfoId);
    Task<List<ContactInfo>> GetContactsByLocationAsync(string location);
}
