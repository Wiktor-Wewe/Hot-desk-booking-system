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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SimpleView", policy => policy.RequireClaim("permissions", ((int)UserPermissions.SimpleView).ToString()));
    options.AddPolicy("AdminView", policy => policy.RequireClaim("permissions", ((int)UserPermissions.AdminView).ToString()));

    options.AddPolicy("CreateEmployee", policy => policy.RequireClaim("permissions", ((int)UserPermissions.CreateEmployee).ToString()));
    options.AddPolicy("UpdateEmployee", policy => policy.RequireClaim("permissions", ((int)UserPermissions.UpdateEmployee).ToString()));
    options.AddPolicy("DeleteEmployee", policy => policy.RequireClaim("permissions", ((int)UserPermissions.DeleteEmployee).ToString()));

    options.AddPolicy("CreateLocation", policy => policy.RequireClaim("permissions", ((int)UserPermissions.CreateLocation).ToString()));
    options.AddPolicy("UpdateLocation", policy => policy.RequireClaim("permissions", ((int)UserPermissions.UpdateLocation).ToString()));
    options.AddPolicy("DeleteLocation", policy => policy.RequireClaim("permissions", ((int)UserPermissions.DeleteLocation).ToString()));

    options.AddPolicy("CreateDesk", policy => policy.RequireClaim("permissions", ((int)UserPermissions.CreateDesk).ToString()));
    options.AddPolicy("UpdateDesk", policy => policy.RequireClaim("permissions", ((int)UserPermissions.UpdateDesk).ToString()));
    options.AddPolicy("DeleteDesk", policy => policy.RequireClaim("permissions", ((int)UserPermissions.DeleteDesk).ToString()));
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
