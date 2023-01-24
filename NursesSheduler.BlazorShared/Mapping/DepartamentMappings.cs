using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.Departaments.Queries.GetAllDepartaments;

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
        }
    }
}
