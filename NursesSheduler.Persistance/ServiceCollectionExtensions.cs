using Microsoft.Extensions.DependencyInjection;
using NursesScheduler.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using NursesScheduler.Persistance.Interfaces;
using System.Security;

namespace NursesScheduler.Persistance
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
           
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
        }
    }
}
