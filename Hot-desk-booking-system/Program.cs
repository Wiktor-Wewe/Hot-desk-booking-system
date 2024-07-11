using Hdbs.Core.ExceptionFilters;
using Hdbs.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Serilog;
using Hdbs.Repositories.Implementations;
using Hdbs.Transfer.Locations.Data;
using Hdbs.Services.Implementations;
using Hot_desk_booking_system.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add reference handler for json serializer and ExceptionFilter
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
}).AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

// Set Database
builder.Services.AddDbContext<HdbsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add mediatr
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Register all repositories and services in assemblies
builder.Services.AddRepositoriesAndServices(
    typeof(LocationRepository).Assembly,
    typeof(LocationService).Assembly
);

// Register all handlers in assemblies
builder.Services.AddMediatRHandlers(
    typeof(LocationDto).GetTypeInfo().Assembly,
    typeof(LocationRepository).GetTypeInfo().Assembly,
    typeof(LocationService).GetTypeInfo().Assembly
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Add Serilog
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
