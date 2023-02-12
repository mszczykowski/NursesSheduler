using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using FluentValidation;
using NursesScheduler.BusinessLogic.Validation;
using NursesScheduler.BusinessLogic.Interfaces.Services;
using NursesScheduler.BusinessLogic.Services;
using NursesScheduler.Domain.DatabaseModels;

namespace NursesScheduler.BusinessLogic
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddTransient<IValidator<Nurse>, NurseValidator>();
            services.AddTransient<IValidator<Departament>, DepartamentValidator>();
            services.AddTransient<ICalendarService, CalendarService>();
            services.AddSingleton<IScheduleConfigurationService, ScheduleConfigurationService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
