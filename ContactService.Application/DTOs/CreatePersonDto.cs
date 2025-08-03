namespace ContactService.Application.DTOs;

public class CreatePersonDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Company { get; set; } = null!;
}
