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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using Hdbs.Core.CustomExceptions;
using Hdbs.Core.Enums;
using Hot_desk_booking_system.DbInitializer;
using Hot_desk_booking_system.PermissionHandler;
using Microsoft.AspNetCore.Authorization;

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

// Add Identity
builder.Services.AddIdentity<Employee, IdentityRole>()
    .AddEntityFrameworkStores<HdbsContext>()
    .AddDefaultTokenProviders();

// Configure Identity
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// Get SecretKey from configuration
var secretKey = builder.Configuration["Jwt:SecretKey"];
if (secretKey == null)
{
    throw new CustomException(CustomErrorCode.NoJwtSecretKey, "Unable to get SecretKey");
}

// Add JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role
    };
});

// Add permissions handler
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

// Add permissions
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SimpleView", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.SimpleView)));
    options.AddPolicy("AdminView", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.AdminView)));

    options.AddPolicy("CreateEmployee", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.CreateEmployee)));
    options.AddPolicy("UpdateEmployee", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.UpdateEmployee)));
    options.AddPolicy("DeleteEmployee", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.DeleteEmployee)));

    options.AddPolicy("CreateLocation", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.CreateLocation)));
    options.AddPolicy("UpdateLocation", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.UpdateLocation)));
    options.AddPolicy("DeleteLocation", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.DeleteLocation)));

    options.AddPolicy("CreateDesk", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.CreateDesk)));
    options.AddPolicy("UpdateDesk", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.UpdateDesk)));
    options.AddPolicy("DeleteDesk", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.DeleteDesk)));

    options.AddPolicy("SetPermissions", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.SetPermissions)));
    options.AddPolicy("SetEmployeeStatus", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.SetEmployeeStatus)));

    options.AddPolicy("CreateReservation", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.CreateReservation)));
    options.AddPolicy("UpdateReservation", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.UpdateReservation)));
    options.AddPolicy("DeleteReservation", policy => policy.Requirements.Add(new PermissionRequirement(UserPermissions.DeleteReservation)));
});

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

// Add Swagger with JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hot desk booking system API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Initialize Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<HdbsContext>();

        DbInitializer.Initialize(services).GetAwaiter().GetResult();

        context.SaveChanges();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while initializing the database.");
        throw;
    }
}

app.Run();
