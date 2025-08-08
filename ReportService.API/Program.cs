using MassTransit;
using Microsoft.EntityFrameworkCore;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.EventHandlers;
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
builder.Services.AddHostedService<ReportBackgroundService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PersonCreatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => { });

        cfg.ReceiveEndpoint("person-created-event-queue", e =>
        {
            e.ConfigureConsumer<PersonCreatedEventConsumer>(context);
        });
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
