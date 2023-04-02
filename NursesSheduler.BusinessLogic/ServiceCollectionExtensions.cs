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
            services.AddTransient<IValidator<Nurse>, NurseValidator>();
            services.AddTransient<IValidator<Departament>, DepartamentValidator>();
            services.AddTransient<IValidator<Absence>, AbsenceValidator>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IHolidaysManager, HolidaysManager>();
            services.AddTransient<IWorkTimeConfigurationManager, WorkTimeConfigurationManager>();
            services.AddTransient<IWorkTimeService, WorkTimeService>();
            services.AddTransient<IMemoryCache, MemoryCache>();
            services.AddTransient<IAbsencesService, AbsencesService>();

            services.AddSingleton<ICurrentDateService, CurrentDateService>();
        }
    }
}
