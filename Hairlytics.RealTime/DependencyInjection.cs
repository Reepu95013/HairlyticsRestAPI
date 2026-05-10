using Hairlytics.RealTime.Interfaces;
using Hairlytics.RealTime.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Hairlytics.RealTime
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRealTime(this IServiceCollection services)
        {
            services.AddSignalR();

            services.AddScoped<IDashboardNotifier,DashboardNotifier>();


            return services;
        }
    }
}
