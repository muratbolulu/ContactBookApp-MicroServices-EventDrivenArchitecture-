using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportService.Domain.Entities;

public class ReportContact
{
    public Guid Id { get; set; }   // Primary key
    public Guid ContactId { get; set; }   // ContactService’den gelen kişi ID

    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // Foreign key
    public Guid ReportId { get; set; }
    public Report Report { get; set; } = null!;
}
