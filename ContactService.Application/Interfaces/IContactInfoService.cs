using ContactService.Application.DTOs;
using ContactService.Domain.Entities;
using SharedKernel.Interface;

namespace ContactService.Application.Interfaces;

public interface IContactInfoService :IGenericService<ContactInfo>
{
    Task AddContactInfoAsync(AddContactInfoDto dto);
    Task RemoveContactInfoAsync(Guid contactInfoId);
}
