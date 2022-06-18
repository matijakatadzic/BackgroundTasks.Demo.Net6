using BackgroundTasks.Demo.Business.Service;
using Microsoft.Extensions.DependencyInjection;

namespace BackgroundTasks.Demo.Business
{
    public static class BusinessServiceExtension
    {
        public static void AddBusiness(this IServiceCollection services)
        {
            services.AddScoped<SampleService>();
            services.AddSingleton<SettingService>();
            services.AddSingleton<PeriodicHostedService>();
            services.AddHostedService(
                provider => provider.GetRequiredService<PeriodicHostedService>());

        }
    }
}