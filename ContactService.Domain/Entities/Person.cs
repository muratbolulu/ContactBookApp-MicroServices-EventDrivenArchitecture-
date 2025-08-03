namespace ContactService.Domain.Entities;

public class Person
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Company { get; set; } = null!;
    public ICollection<ContactInfo> ContactInfos { get; set; } = new List<ContactInfo>();
}
