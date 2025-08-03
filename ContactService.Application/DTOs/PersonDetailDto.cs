namespace ContactService.Application.DTOs;

public class PersonDetailDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Company { get; set; } = null!;
    public List<ContactInfoDto> ContactInfos { get; set; } = new();
}
