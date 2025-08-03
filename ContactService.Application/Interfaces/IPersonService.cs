using ContactService.Application.DTOs;

namespace ContactService.Application.Interfaces;

public interface IPersonService
{
    Task<Guid> CreatePersonAsync(CreatePersonDto dto);
    Task DeletePersonAsync(Guid id);
    Task<List<PersonDetailDto>> GetAllPersonsAsync();
    Task<PersonDetailDto> GetPersonWithContactsAsync(Guid id);
}
