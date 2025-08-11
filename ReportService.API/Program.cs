using ContactService.Domain.Entities;
using ContactService.Infrastructure.Persistence;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using ReportService.Application.Mappings;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.EventHandlers;
using ReportService.Infrastructure.NewFolder.Services;
using ReportService.Infrastructure.Persistence;
using ReportService.Infrastructure.Repositories;
using SharedKernel.Events;
using SharedKernel.Infrastructure;
using SharedKernel.Interface;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ReportDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.Load("ReportService.Application"));
});

builder.Services.AddAutoMapper(cfg => {
    cfg.AddMaps(typeof(ReportProfile).Assembly);
});

//builder.Services.AddScoped<IGenericRepository<Report>, GenericRepository<Report>>();
builder.Services.AddScoped<IGenericRepository<Report>, GenericRepository<Report, ReportDbContext>>();
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
app.UseRouting();

app.MapControllers();

app.Run();

//test. 
var harness = new InMemoryTestHarness();

// Tüketiciyi ekle
var consumerHarness = harness.Consumer<PersonCreatedEventConsumer>();

await harness.Start();
try
{
    await harness.InputQueueSendEndpoint.Send(new PersonCreatedEvent {  });

    Assert.True(await harness.Consumed.S elect<PersonCreatedEvent>().Any());
    Assert.True(await consumerHarness.Consumed.Select<PersonCreatedEvent>().Any());
}
finally
{
    await harness.Stop();
}