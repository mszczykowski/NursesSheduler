using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.Infrastructure.HttpClients;
using System.Configuration;
using NursesScheduler.Infrastructure.Context;
using NursesScheduler.BusinessLogic.Abstractions.Infrastructure;

namespace NursesScheduler.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
            
            services.AddTransient<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            
            services.AddHttpClient<IHolidaysApiClient, HolidaysApiClient>(client =>
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["calendarApi"]);
            });
        }
    }
}
