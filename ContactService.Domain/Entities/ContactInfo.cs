using SharedKernel.Enums;

namespace ContactService.Domain.Entities;

public class ContactInfo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PersonId { get; set; }
    public ContactType Type { get; set; }
    public string Value { get; set; } = null!;

    public Person? Person { get; set; }
}
