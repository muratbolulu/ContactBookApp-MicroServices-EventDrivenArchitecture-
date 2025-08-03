using Microsoft.EntityFrameworkCore;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Persistence;
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



var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();
