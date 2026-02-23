using Hairlytics.Application.ApplicationHelper;
using Hairlytics.Application.Mapping;
using Hairlytics.Infrastructure.Database;
using Hairlytics.Infrastructure.Extensions;
using Hairlytics.WebApp.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Bind Sql Database
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// add service and repository

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureRepository();

// add helper class and interface
builder.Services.AddSingleton<IPasswordHasher>(sp => new BcryptPasswordHasher(workFactor: 12));

// ? AutoMapper (Clean Architecture)
builder.Services.AddAutoMapper(typeof(MappingProfile));


// Add Authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AuthCookie";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(20);
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Error";
    });


builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
