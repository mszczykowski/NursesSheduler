using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.Infrastructure.HttpClients;
using System.Configuration;
using NursesScheduler.BusinessLogic.Interfaces.Infrastructure;
using NursesScheduler.Infrastructure.Context;

namespace NursesScheduler.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddHttpClient<IHolidaysApiClient, HolidaysApiClient>(client =>
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["calendarApi"]);
            });
        }
    }
}
