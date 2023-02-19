using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.CreateDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Commands.EditDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetAllDepartaments;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Departaments.Queries.GetDepartament;

namespace NursesScheduler.BlazorShared.Mapping
{
    public class DepartamentViewModelMappings : Profile
    {
        public DepartamentViewModelMappings()
        {
            CreateMap<DepartamentViewModel, GetAllDepartamentsResponse>()
                .ReverseMap();
            CreateMap<DepartamentViewModel, GetAllDepartamentsRequest>()
                .ReverseMap();
            CreateMap<DepartamentViewModel, GetDepartamentResponse>()
                .ReverseMap();
            CreateMap<DepartamentViewModel, GetDepartamentRequest>()
                .ReverseMap();
            CreateMap<DepartamentViewModel, EditDepartamentRequest>()
                .ReverseMap();
            CreateMap<DepartamentViewModel, CreateDepartamentRequest>()
                .ReverseMap();
        }
    }
}
