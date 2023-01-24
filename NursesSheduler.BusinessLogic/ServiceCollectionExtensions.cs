using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using FluentValidation;
using NursesScheduler.BusinessLogic.Validation.Nurse;
using NursesScheduler.BusinessLogic.Nurses.Commands.CreateNurse;
using NursesScheduler.BusinessLogic.Departaments.Commands.CreateDepartament;
using NursesScheduler.BusinessLogic.Validation.Departament;

namespace NursesScheduler.BusinessLogic
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusinessLogicLayer(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateNurseRequest>, CreateNurseRequestValidator>();
            services.AddScoped<IValidator<CreateDepartamentRequest>, CreateDepartamentRequestValidator>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
