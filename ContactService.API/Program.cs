using AutoMapper;
using ContactService.Application.Interfaces;
using ContactService.Application.Mappings;
using ContactService.Domain.Entities;
using ContactService.Infrastructure.Messaging;
using ContactService.Infrastructure.Persistence;
using ContactService.Infrastructure.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Infrastructure;
using SharedKernel.Interface;
using SharedKernel.Messaging;
using SharedKernel.Messaging.Abstraction;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// DbContext
builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddValidatorsFromAssembly(Assembly.Load("ContactService.Application"));

//builder.Services.AddAutoMapper(typeof(Profile).Assembly);
builder.Services.AddAutoMapper(typeof(PersonProfile).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.Load("ContactService.Application"));
});

// Register Application Services
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IContactInfoService, ContactInfoService>();
builder.Services.AddScoped<IGenericRepository<Person>, GenericRepository<Person>>();
builder.Services.AddScoped<IGenericRepository<ContactInfo>, GenericRepository<ContactInfo>>();
builder.Services.AddSingleton<IEventBus, RabbitMQProducer>();
//builder.Services.AddSingleton<IRabbitMQProducer, RabbitMQProducer>();

builder.Services.AddControllers();
//builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(Assembly.Load("ContactService.Application"));

// Register Swagger
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

app.MapControllers();

app.Run();
