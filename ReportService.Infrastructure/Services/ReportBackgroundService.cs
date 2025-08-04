using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReportService.Domain.Entities;
using ReportService.Domain.Enums;
using SharedKernel.Interface;
using System.Text.Json;

namespace ReportService.Infrastructure.NewFolder.Services;

public class ReportBackgroundService : BackgroundService
{
    private readonly ILogger<ReportBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ReportBackgroundService(ILogger<ReportBackgroundService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Report background service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var reportRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Report>>();

            var pendingReports = await reportRepository.GetWhereAsync(r => r.Status == ReportStatus.Preparing);

            foreach (var report in pendingReports)
            {
                // Burada: Lokasyon bazlı kişi ve iletişim bilgisi analizleri yapılacak (mock ile geçebiliriz)
                var reportData = new
                {
                    Location = report.Location,
                    PersonCount = new Random().Next(1, 100),
                    PhoneCount = new Random().Next(1, 100)
                };

                report.Content = JsonSerializer.Serialize(reportData);
                report.Status = ReportStatus.Completed;

                await reportRepository.UpdateAsync(report);
                await reportRepository.SaveChangesAsync();

                _logger.LogInformation($"Report {report.Id} completed.");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
