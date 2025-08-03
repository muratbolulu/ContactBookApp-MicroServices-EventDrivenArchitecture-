namespace ContactService.Application.DTOs;

public class ContactInfoDto
{
    public Guid Id { get; set; }
    public string Value { get; set; } = null!;
    public string Type { get; set; } = null!;
}
