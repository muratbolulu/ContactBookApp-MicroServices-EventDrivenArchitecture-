using SharedKernel.Enums;

namespace ContactService.Application.DTOs;

public class AddContactInfoDto
{
    public Guid PersonId { get; set; }
    public ContactType Type { get; set; }  // Telefon, Email, Konum, vb
    public string Value { get; set; } = null!;
}
