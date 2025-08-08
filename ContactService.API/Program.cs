using ContactService.Application.Interfaces;
using ContactService.Application.Mappings;
using ContactService.Domain.Entities;
using ContactService.Infrastructure.Persistence;
using ContactService.Infrastructure.Services;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure;
using SharedKernel.Interface;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.Load("ContactService.Application"));

// AutoMapper
builder.Services.AddAutoMapper(cfg => {
    cfg.AddMaps(typeof(PersonProfile).Assembly);
});

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.Load("ContactService.Application"));
});

// Application Services
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IContactInfoService, ContactInfoService>();
builder.Services.AddScoped<IGenericRepository<Person>, GenericRepository<Person>>();
builder.Services.AddScoped<IGenericRepository<ContactInfo>, GenericRepository<ContactInfo>>();

// MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/", async (IPublishEndpoint publishEndpoint) =>
{
    await publishEndpoint.Publish(new Object {/* Text = "Hello MassTransit!" */});
    return Results.Ok("Message published!");
});

app.MapControllers();

app.Run();

