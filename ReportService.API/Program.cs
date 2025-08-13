using MassTransit;
using Microsoft.EntityFrameworkCore;
using ReportService.Application.Mappings;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.EventHandlers;
using ReportService.Infrastructure.NewFolder.Services;
using ReportService.Infrastructure.Persistence;
using SharedKernel.Infrastructure;
using SharedKernel.Interface;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// DbContext
builder.Services.AddDbContext<ReportDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
builder.Services.AddScoped<IGenericRepository<Report>, GenericRepository<Report, ReportDbContext>>();

// Hosted Service
builder.Services.AddHostedService<ReportBackgroundService>();

// MassTransit + RabbitMQ
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