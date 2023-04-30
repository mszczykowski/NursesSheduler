using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using FluentValidation;
using NursesScheduler.BusinessLogic.Validation;
using NursesScheduler.BusinessLogic.Services;
using NursesScheduler.Domain.DomainModels;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Managers;
using NursesScheduler.BusinessLogic.Managers;
using Microsoft.Extensions.Caching.Memory;

namespace NursesScheduler.BusinessLogic
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient<IMemoryCache, MemoryCache>();

            //validators
            services.AddTransient<IValidator<Nurse>, NurseValidator>();
            services.AddTransient<IValidator<Departament>, DepartamentValidator>();
            services.AddTransient<IValidator<Absence>, AbsenceValidator>();
            services.AddTransient<IValidator<AbsencesSummary>, AbsenceSummaryValidator>();
            services.AddTransient<IValidator<DepartamentSettings>, DepartamentSettingsValidator>();

            //managers
            services.AddTransient<IHolidaysManager, HolidaysManager>();
            services.AddTransient<IDepartamentSettingsManager, DepartamentSettingsManager>();

            //services
            services.AddTransient<IWorkTimeService, WorkTimeService>();
            services.AddTransient<IAbsencesService, AbsencesService>();
            services.AddSingleton<ICurrentDateService, CurrentDateService>();
            services.AddTransient<ISchedulesService, SchedulesService>();
            services.AddTransient<ICalendarService, CalendarService>();
        }
    }
}
