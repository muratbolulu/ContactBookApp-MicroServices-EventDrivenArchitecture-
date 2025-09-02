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
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReportService API", Version = "v1" });
//});

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
builder.Services.AddMassTransit(cfg =>
{
    cfg.AddConsumer<ReportContactsPreparedConsumer>();

    cfg.UsingRabbitMq((context, config) =>
    {
        //config.Host("rabbitmq://localhost");

        config.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

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
    //app.UseSwagger();
    //app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}


// Configure the HTTP request pipeline.
app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();

