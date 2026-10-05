using System.Text;
using EkubCircle.Application.Interfaces;
using EkubCircle.Infrastructure.Authentication;
using EkubCircle.Infrastructure.Identity;
using EkubCircle.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using EkubCircle.Infrastructure.Persistence.Context; // ApplicationDbContext
using Scalar.AspNetCore; // <--- Added for Scalar UI

var builder = WebApplication.CreateBuilder(args);

// 1. Configure JWT Options
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

// Update your DbContext registration in Program.cs
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IApplicationDbContext>(provider => 
    provider.GetRequiredService<ApplicationDbContext>());

// 3. Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 4. Configure Services
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

// 5. Configure Authentication & JwtBearer
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
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
        ValidIssuer = jwtSettings!.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
    };
});

// 6. Configure MediatR & FluentValidation
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(EkubCircle.Application.Commands.CreateCircleCommand).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(EkubCircle.Application.Commands.CreateCircleCommand).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // <--- Renders the Scalar Interactive UI
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();