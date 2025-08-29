using ContactService.Application.Interfaces;
using ContactService.Application.Mappings;
using ContactService.Application.Services;
using ContactService.Domain.Entities;
using ContactService.Infrastructure.Messaging.Consumers;
using ContactService.Infrastructure.Messaging.Services;
using ContactService.Infrastructure.Persistence;
using ContactService.Infrastructure.Persistence.Repositories;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
         b => b.MigrationsAssembly("ContactService.Infrastructure") // Migration dosyalar�n� buraya ekle
        ));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.Load("ContactService.Application"));

// AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(PersonProfile).Assembly);
    cfg.AddMaps(typeof(ContactInfoProfile).Assembly);
});

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.Load("ContactService.Application"));
});

// Application Services
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IContactInfoService, ContactInfoService>();
builder.Services.AddScoped<IGenericRepository<Person>, GenericRepository<Person>>(); //generic hal i�in tekrar bak�lacak
builder.Services.AddScoped<IGenericRepository<ContactInfo>, GenericRepository<ContactInfo>>();  //generic hal i�in tekrar bak�lacak
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ReportCreatedEventConsumer>();

//// MassTransit with RabbitMQ
//builder.Services.AddMassTransit(x =>
//{
//    x.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host("localhost", "/", h =>
//        {
//            h.Username("guest");
//            h.Password("guest");
//        });
//    });
//});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ReportCreatedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost");

        cfg.ReceiveEndpoint("report-created-queue", e =>
        {
            e.ConfigureConsumer<ReportCreatedEventConsumer>(context);
        });
    });
});

// Controllers
//string-enum d�n���mleri i�in JsonStringEnumConverter ekle
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.MapGet("/", async (IPublishEndpoint publishEndpoint) =>
//{
//    await publishEndpoint.Publish(new MyMessage { Text = "Hello MassTransit!" });
//    return Results.Ok("Message published!");
//});

app.MapControllers();

app.Run();

