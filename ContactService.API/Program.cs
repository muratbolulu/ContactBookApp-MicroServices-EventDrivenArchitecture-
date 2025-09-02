using ContactService.Application.Features.Persons.Handlers.QueryHandlers;
using ContactService.Application.Features.Reports.Consumers;
using ContactService.Application.Interface;
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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ContactDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
         b => b.MigrationsAssembly("ContactService.Infrastructure") // Migration dosyalarýný buraya ekle
        ));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.Load("ContactService.Application"));

//AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(typeof(PersonProfile).Assembly);
    cfg.AddMaps(typeof(ContactInfoProfile).Assembly);
});

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetAllPersonsQueryHandler).Assembly));

// Repositories Registrations
builder.Services.AddScoped<IGenericRepository<Person>, GenericRepository<Person>>(); //generic hal için tekrar bakýlacak
builder.Services.AddScoped<IGenericRepository<ContactInfo>, GenericRepository<ContactInfo>>();  //generic hal için tekrar bakýlacak
builder.Services.AddScoped<IContactInfoRepository, ContactInfoRepository>();


//Application Services Registrations
//builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
builder.Services.AddScoped<IGenericService<Person>, GenericService<Person>>();  //from CreatePersonCommandHandler
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IContactInfoService, ContactInfoService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ReportCreatedEventConsumer>(); // ??

//// MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ReportRequestedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("report-contacts-prepared-queue", e =>
        {
            e.ConfigureConsumer<ReportRequestedEventConsumer>(context);
            e.Durable = true; // queue kalýcý
        });
    });
});

// Controllers
//string-enum dönüþümleri için JsonStringEnumConverter ekle
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContactService API", Version = "v1" });
});

var app = builder.Build();

//Middleware 
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

