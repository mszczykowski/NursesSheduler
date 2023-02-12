using Microsoft.Extensions.DependencyInjection;
using NursesScheduler.BlazorShared.Stores;
using System.Reflection;

namespace NursesScheduler.BlazorShared
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPresentationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSingleton<CurrentDepartamentStore>();
        }
    }
}
