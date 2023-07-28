using Append.Blazor.Printing;
using Microsoft.Extensions.DependencyInjection;
using NursesScheduler.BlazorShared.Abstracions;
using NursesScheduler.BlazorShared.Helpers;
using NursesScheduler.BlazorShared.Stores;
using System.Reflection;

namespace NursesScheduler.BlazorShared
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPresentationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IExceptionHandler, ExceptionHandler>();
            services.AddSingleton<CurrentDepartamentStore>();
            services.AddSingleton<ExceptionStore>();

            services.AddScoped<IPrintingService, PrintingService>();
        }
    }
}
