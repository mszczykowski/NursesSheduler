using Microsoft.Extensions.DependencyInjection;
using NursesSheduler.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using NursesSheduler.Persistance.Interfaces;

namespace NursesSheduler.Persistance
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=Scheduler.db"));
           
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
        }
    }
}
