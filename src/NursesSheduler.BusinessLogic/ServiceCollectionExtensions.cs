using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using FluentValidation;
using NursesScheduler.BusinessLogic.Validation;
using NursesScheduler.BusinessLogic.Services;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence;
using NursesScheduler.BusinessLogic.Abstractions.Solver;
using NursesScheduler.BusinessLogic.Solver;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.EditAbsence;

namespace NursesScheduler.BusinessLogic
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //validators
            services.AddTransient<IValidator<Nurse>, NurseValidator>();
            services.AddTransient<IValidator<Departament>, DepartamentValidator>();
            services.AddTransient<IValidator<AddAbsenceRequest>, AddAbsenceRequestValidator>();
            services.AddTransient<IValidator<EditAbsenceRequest>, EditAbsenceRequestValidator>();
            services.AddTransient<IValidator<AbsencesSummary>, AbsenceSummaryValidator>();
            services.AddTransient<IValidator<DepartamentSettings>, DepartamentSettingsValidator>();
            services.AddTransient<IValidator<MorningShift>, MorningShiftValidator>();

            //services
            services.AddSingleton<ICurrentDateService, CurrentDateService>();
            services.AddSingleton<ISeedService, SeedService>();
            services.AddSingleton<ISolverLoggerService, SolverLoggerService>();

            services.AddTransient<IWorkTimeService, WorkTimeService>();
            services.AddTransient<IAbsencesService, AbsencesService>();
            services.AddTransient<ISchedulesService, SchedulesService>();
            services.AddTransient<ICalendarService, CalendarService>();
            services.AddTransient<IScheduleStatsService, ScheduleStatsService>();
            services.AddTransient<IActiveNursesService, ActiveNursesService>();
            services.AddTransient<IQuarterStatsService, QuarterStatsService>();
            services.AddTransient<IScheduleSolverService, ScheduleSolverService>();

            //solver
            services.AddTransient<IScheduleSolver, ScheduleSolver>();
        }
    }
}
