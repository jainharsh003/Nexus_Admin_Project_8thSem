using FluentValidation;
using FluentValidation.AspNetCore;
using LoginSignup.Data;
using LoginSignup.DTOs;
using LoginSignup.Helpers;
using LoginSignup.Mappings;
using LoginSignup.Repositories.Implementations;
using LoginSignup.Repositories.Interfaces;
using LoginSignup.Services.Implementations;
using LoginSignup.Services.Interfaces;
using LoginSignup.Validators;
using LoginSignupBackend.Middlewares;
using LoginSignupBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;

// ✅ Configure Serilog FIRST before builder
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // ✅ Change from Error to Debug temporarily to verify it works
    .WriteTo.Console()
    .WriteTo.File(
        path: Path.Combine(Directory.GetCurrentDirectory(), "Logs", "exceptions.txt"), // ✅ Absolute path
        rollingInterval: RollingInterval.Day,
        outputTemplate: "============================================================\n" +
                        "Timestamp  : {Timestamp:yyyy-MM-dd HH:mm:ss} UTC\n" +
                        "Level      : {Level:u3}\n" +
                        "Message    : {Message:lj}\n" +
                        "Exception  : {Exception}\n" +
                        "============================================================\n\n"
    )
    .CreateLogger();


var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog();
// ===========================
// Database
// ===========================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===========================
// Repositories
// ===========================
builder.Services.AddScoped<IUserRepository, UserRepository>();

// ===========================
// Services
// ===========================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserService, UserService>();

// ===========================
// Validators
// ===========================
builder.Services.AddScoped<IValidator<UserRegisterDto>, UserRegisterValidator>();
builder.Services.AddScoped<IValidator<LoginDto>, LoginValidator>();
builder.Services.AddScoped<IEmailSender, EmailSender>();


builder.Services.AddScoped<IValidator<UpdateUserDto>, UpdateUserValidator>();

// ===========================
// AutoMapper
// ===========================
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// ===========================
// Controllers + FluentValidation
// ===========================
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

// ===========================
// JWT Authentication
// ===========================
var jwtSection = builder.Configuration.GetSection("Jwt");

var jwtKey = jwtSection["Key"];
var jwtIssuer = jwtSection["Issuer"];
var jwtAudience = jwtSection["Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // for localhost only
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        ),

        ClockSkew = TimeSpan.Zero,

        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };
});

// ===========================
// Authorization
// ===========================
builder.Services.AddAuthorization();

// ===========================
// Swagger + JWT Support
// ===========================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter: Bearer {your JWT token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

// ===========================
// CORS
// ===========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ===========================
// Build App
// ===========================
var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

// ===========================
// Middleware
// ===========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
Log.CloseAndFlush();
