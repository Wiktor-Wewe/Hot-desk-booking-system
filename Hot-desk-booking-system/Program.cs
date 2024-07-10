using Hdbs.Core.ExceptionFilters;
using Hdbs.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Serilog;
using Hdbs.Repositories.Interfaces;
using Hdbs.Repositories.Implementations;
using Hdbs.Transfer.Shared.Data;
using MediatR;
using Hdbs.Transfer.Locations.Queries;
using Hdbs.Transfer.Locations.Data;
using Hdbs.Repositories.Handlers;
using Hdbs.Services.Interfaces;
using Hdbs.Services.Implementations;
using Hdbs.Transfer.Locations.Commands;
using Hdbs.Services.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddDbContext<HdbsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// add mediatr
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// repositories
builder.Services.AddScoped<ILocationRepository, LocationRepository>();

// services
builder.Services.AddScoped<ILocationService, LocationService>();

// location queries and commands
builder.Services.AddTransient<IRequestHandler<ListLocationsQuery, PaginatedList<LocationListDto>>, ListLocationHandler>();
builder.Services.AddTransient<IRequestHandler<GetLocationQuery, LocationDto>, GetLocationHandler>();
builder.Services.AddTransient<IRequestHandler<CreateLocationCommand, LocationDto>, CreateLocationHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateLocationCommand>, UpdateLocationHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteLocationCommand>, DeleteLocationHandler>();

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
