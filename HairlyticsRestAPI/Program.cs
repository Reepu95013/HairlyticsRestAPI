using Hairlytics.Application.ApplicationHelper;
using Hairlytics.Application.Mapping;
using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Application.Services;
using Hairlytics.Domain.Interfaces;
using Hairlytics.Infrastructure.Database;
using Hairlytics.Infrastructure.Extensions;
using Hairlytics.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bind Sql Database
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// add service and repository

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureRepository();

// add helper class and interface
builder.Services.AddSingleton<IPasswordHasher>(sp => new BcryptPasswordHasher(workFactor: 12));

// ✅ AutoMapper (Clean Architecture)
builder.Services.AddAutoMapper(typeof(MappingProfile));

//JWT Service

var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.RequireHttpsMetadata = false;
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("V3ryL0ngAndC0mpl3xS3cr3tK3y256df")),

    };
});

builder.Services.AddAuthorization();





var app = builder.Build();

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

app.Run();
