using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.Infrastructure.HttpClients;
using System.Configuration;
using NursesScheduler.Infrastructure.Context;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.Infrastructure.Providers;

namespace NursesScheduler.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IMemoryCache, MemoryCache>();

            //db context
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
            services.AddTransient<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            //providers
            services.AddTransient<IHolidaysProvider, HolidaysProvider>();
            services.AddTransient<IDepartamentSettingsProvider, DepartamentSettingsProvider>();

            //http clients
            services.AddHttpClient<IHolidaysApiClient, HolidaysApiClient>(client =>
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["calendarApi"]);
            });
        }
    }
}
