﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using FluentValidation;
using NursesScheduler.BusinessLogic.Validation;
using NursesScheduler.BusinessLogic.Services;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Absences.Commands.AddAbsence;

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
            services.AddTransient<IValidator<AbsencesSummary>, AbsenceSummaryValidator>();
            services.AddTransient<IValidator<DepartamentSettings>, DepartamentSettingsValidator>();

            //services
            services.AddSingleton<ICurrentDateService, CurrentDateService>();
            services.AddSingleton<ISeedService, SeedService>();
            services.AddTransient<IWorkTimeService, WorkTimeService>();
            services.AddTransient<IAbsencesService, AbsencesService>();
            services.AddTransient<ISchedulesService, SchedulesService>();
            services.AddTransient<ICalendarService, CalendarService>();
            services.AddTransient<INurseStatsService, NurseStatsService>();
            services.AddTransient<ISolverService, SolverService>();
        }
    }
}
