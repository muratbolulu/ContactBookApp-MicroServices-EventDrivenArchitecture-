using ContactService.Application.DTOs;

namespace ContactService.Application.Interfaces;

public interface IContactInfoService
{
    Task AddContactInfoAsync(AddContactInfoDto dto);
    Task RemoveContactInfoAsync(Guid contactInfoId);
}
