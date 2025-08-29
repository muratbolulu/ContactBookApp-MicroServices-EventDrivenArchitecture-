using MassTransit;
using Microsoft.EntityFrameworkCore;
using ReportService.Application.Features.Reports.Consumers;
using ReportService.Application.Interfaces;
using ReportService.Application.Mappings;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.NewFolder.Services;
using ReportService.Infrastructure.Persistence;
using ReportService.Infrastructure.Persistence.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// DbContext
builder.Services.AddDbContext<ReportDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("ReportService.Infrastructure") // Migration dosyalarýný buraya ekle
    ));

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.Load("ReportService.Application"));
});

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(ReportProfile).Assembly);
});

// Repository
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IGenericRepository<Report>, GenericRepository<Report>>();

// Hosted Service
builder.Services.AddHostedService<ReportBackgroundService>();

//// MassTransit + RabbitMQ
//builder.Services.AddMassTransit(x =>
//{
//    x.AddConsumer<PersonCreatedEventConsumer>();

//    x.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host("localhost", "/", h => { });

//        cfg.ReceiveEndpoint("person-created-event-queue", e =>
//        {
//            e.ConfigureConsumer<PersonCreatedEventConsumer>(context);
//        });
//    });
//});

builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<ReportContactsPreparedConsumer>();

    cfg.UsingRabbitMq((context, config) =>
    {
        config.Host("rabbitmq://localhost");

        config.ReceiveEndpoint("report-contacts-prepared-queue", e =>
        {
            e.ConfigureConsumer<ReportContactsPreparedConsumer>(context);
        });
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


// Configure the HTTP request pipeline.
app.UseRouting();

app.MapControllers();

app.Run();