using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.AddNurse;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNursesFromDepartament;

namespace NursesScheduler.BlazorShared.Mapping
{
    public class NurseViewModelMappings : Profile
    {
        public NurseViewModelMappings()
        {
            CreateMap<NurseViewModel, AddNurseRequest>()
                .ReverseMap();
            CreateMap<NurseViewModel, AddNurseResponse>()
                .ReverseMap();
            CreateMap<NurseViewModel, GetNursesFromDepartamentResponse>()
                .ReverseMap();
        }
    }
}
