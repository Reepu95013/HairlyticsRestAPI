using Hairlytics.Application.ApplicationHelper;
using Hairlytics.Application.Mapping;
using Hairlytics.Domain.Enums;
using Hairlytics.Infrastructure.Database;
using Hairlytics.Infrastructure.Extensions;
using Hairlytics.WebApp.Components;
using Hairlytics.WebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Bind Sql Database
builder.Services.AddDbContext<ApplicationDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// add service and repository
builder.Services.AddScoped<LoadingService>();
builder.Services.AddScoped<AppSettingsFileService>();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureRepository();
builder.Services.AddHttpContextAccessor();

// add helper class and interface
builder.Services.AddSingleton<IPasswordHasher>(sp => new BcryptPasswordHasher(workFactor: 12));

// ? AutoMapper (Clean Architecture)
builder.Services.AddAutoMapper(typeof(MappingProfile));

// add Blazor.Bootstrap

builder.Services.AddBlazorBootstrap();




var sessionMinutes = builder.Configuration.GetValue<int>("AdminSettings:SessionTimeoutMinutes", 20);

// Add Authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "AuthCookie";
        options.Cookie.MaxAge = TimeSpan.FromMinutes(sessionMinutes);
        options.Cookie.HttpOnly = true;
        options.LoginPath = "/Admin/Login";
        options.AccessDeniedPath = "/Error";
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole(
            nameof(UserRole.Admin),
            nameof(UserRole.SubAdmin));
    });
});


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


//var basePath = builder.Configuration["FileStorage:BasePath"];

//if (string.IsNullOrEmpty(basePath))
//{
//    throw new Exception("FileStorage:BasePath is not configured");
//}

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(basePath),
//    RequestPath = "/HairlyticsStorage"
//});


app.UseStaticFiles();

// 2. Enable your custom storage mapping
var basePath = builder.Configuration["FileStorage:BasePath"];
if (!string.IsNullOrEmpty(basePath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(basePath),
        RequestPath = "/HairlyticsStorage"
    });
}


app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
