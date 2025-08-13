using Microsoft.EntityFrameworkCore;
using ReportService.Domain.Entities;

namespace ReportService.Infrastructure.Persistence;

public class ReportDb : DbContext
{
    public ReportDb(DbContextOptions<ReportDb> options) : base(options) { }

    public DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Report>().ToTable("Reports");
        modelBuilder.Entity<Report>().Property(r => r.Status).HasConversion<int>();

    }
}
