using Microsoft.EntityFrameworkCore;
using ReportService.Application.EventHandlers;
using ReportService.Domain.Entities;
using ReportService.Domain.Interfaces;
using ReportService.Infrastructure.Messaging;
using ReportService.Infrastructure.NewFolder.Services;
using ReportService.Infrastructure.Persistence;
using ReportService.Infrastructure.Repositories;
using SharedKernel.Infrastructure;
using SharedKernel.Interface;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ReportDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.Load("ReportService.Application"));
});


builder.Services.AddScoped<IGenericRepository<Report>, GenericRepository<Report>>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddHostedService<ReportBackgroundService>();
builder.Services.AddHostedService<RabbitMQConsumerService>();
builder.Services.AddHostedService<PersonCreatedEventConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
