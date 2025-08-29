//using ContactService.Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using ReportService.Domain.Entities;

//namespace SharedKernel.Persistence.Configurations;

//public class ReportConfiguration : IEntityTypeConfiguration<Report>
//{
//    public void Configure(EntityTypeBuilder<Report> builder)
//    {
//        builder.HasKey(p => p.Id);
//        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
//        builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
//        builder.Property(p => p.Company).IsRequired().HasMaxLength(100);
//    }
//}

