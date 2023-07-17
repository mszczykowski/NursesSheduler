using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.CreateDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.EditDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.PickDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetAllDepartaments;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetDepartament;

namespace NursesScheduler.BlazorShared.Mappings
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
