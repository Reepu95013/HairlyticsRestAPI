using Hairlytics.Application.ServiceInterfaces;
using Hairlytics.Application.Services;
using Hairlytics.Domain.Interfaces;
using Hairlytics.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hairlytics.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {

            public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            {
                services.AddScoped<IJwtService, JwtService>();
                services.AddScoped<IAuthService, AuthService>();
                services.AddScoped<IUserService, UserService>();
                services.AddScoped<IVendorService, VendorService>();
                services.AddScoped<IEmailService, EmailService>();
                services.AddScoped<ICategoryService, CategoryService>();

                return services;
            }


            public static IServiceCollection AddInfrastructureRepository(this IServiceCollection services)
            {
                services.AddScoped<IAuthRepository, AuthRepository>();
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<ICategoryRepository, CategoryRepository>();
                services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
                return services;
            }

       
    }
}
