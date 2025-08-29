using ContactService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Infrastructure.Persistence;

public class ContactDbContext : DbContext
{
    public ContactDbContext(DbContextOptions<ContactDbContext> options)
        : base(options) { }

    public DbSet<Person> Persons => Set<Person>();
    public DbSet<ContactInfo> ContactInfos => Set<ContactInfo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //PersonConfiguration, ContactInfoConfiguration için topluca ekler.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}

