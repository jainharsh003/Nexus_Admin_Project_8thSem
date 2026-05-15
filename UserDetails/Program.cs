using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Service2.Repositories;
using System.Text;
using UserDetails.Clients.Implementation;
using UserDetails.Clients.Interface;
using UserDetails.Data;
using UserDetails.Middlewares;
using UserDetails.Repositories.Implementation;
using UserDetails.Repositories.Interface;
using UserDetails.Services;
using UserDetails.Services.Implementation;
using UserDetails.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// =============================
// 🔵 LOGGING (Serilog)
// =============================
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// =============================
// 🔵 DATABASE
// =============================
builder.Services.AddDbContext<UserDetailsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// =============================
// 🔵 DEPENDENCY INJECTION
// =============================
builder.Services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();
builder.Services.AddScoped<IUserDetailsService, UserDetailsService>();
builder.Services.AddScoped<IEmploymentRepository, EmploymentRepository>();
builder.Services.AddScoped<IEmploymentService, EmploymentService>();
builder.Services.AddHttpClient();
// =============================
// 🔵 HTTP CLIENT → SERVICE-1
// =============================
builder.Services.AddHttpClient<IAuthClient, AuthClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7218"); // 🔥 Service-1
});

// =============================
// 🔵 JWT AUTHENTICATION (same secret as Service-1)
// =============================
var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"])
            ),

            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();



// =============================
// 🔵 CONTROLLERS
// =============================
builder.Services.AddControllers();

// =============================
// 🔵 SWAGGER + JWT BUTTON
// =============================
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "UserDetails API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter token like: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
// =============================
// 🔵 CORS
// =============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// =============================
// 🔵 BUILD PIPELINE
// =============================
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");


// 🔥 IMPORTANT ORDER
app.UseAuthentication();
app.UseMiddleware<TokenValidationMiddleware>(); 
app.UseAuthorization();

app.MapControllers();

app.Run();