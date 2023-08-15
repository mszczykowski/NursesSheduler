using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.CreateDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.EditDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetAllDepartaments;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.PickDepartament;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal class DepartamentViewModelMappings : Profile
    {
        public DepartamentViewModelMappings()
        {
            CreateMap<GetAllDepartamentsResponse, DepartamentViewModel>();
            CreateMap<GetDepartamentResponse, DepartamentViewModel>();
            CreateMap<DepartamentViewModel, EditDepartamentRequest>();
            CreateMap<DepartamentViewModel, CreateDepartamentRequest>();
            CreateMap<PickDepartamentResponse, DepartamentViewModel>();
        }
    }
}
