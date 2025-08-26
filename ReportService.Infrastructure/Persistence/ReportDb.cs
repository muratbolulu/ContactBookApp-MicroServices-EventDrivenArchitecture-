using Microsoft.EntityFrameworkCore;
using ReportService.Domain.Entities;

namespace ReportService.Infrastructure.Persistence;

public class ReportDb : DbContext
{
    public ReportDb(DbContextOptions<ReportDb> options) : base(options) { }

    public DbSet<Report> Reports { get; set; }
    public DbSet<ReportContact> ReportContacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Report>().ToTable("Reports");
        modelBuilder.Entity<Report>().Property(r => r.Status).HasConversion<int>();
        modelBuilder.Entity<ReportContact>()
                .HasOne(rc => rc.Report)
                .WithMany(r => r.Contacts)
                .HasForeignKey(rc => rc.ReportId);

    }
}
