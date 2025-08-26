using ContactService.Application.DTOs;
using ContactService.Domain.Entities;
using SharedKernel.Interface;

namespace ContactService.Application.Interfaces;

public interface IPersonService :IGenericService<Person>
{
    Task<Guid> CreatePersonAsync(CreatePersonDto dto);
    Task DeletePersonAsync(Guid id);
    Task<List<PersonDetailDto>> GetAllPersonsAsync();
    Task<PersonDetailDto> GetPersonWithContactsAsync(Guid id);
}
