namespace ContactService.Application.DTOs;

public class CreateContactInfoDto
{
    public Guid PersonId { get; set; }
    public string InfoType { get; set; } = string.Empty; // Telefon, Email, Konum
    public string Content { get; set; } = string.Empty;
}
