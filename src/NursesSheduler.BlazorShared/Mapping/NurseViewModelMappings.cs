using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.AddNurse;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Commands.EditNurse;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNurse;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Nurses.Queries.GetNursesFromDepartament;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal class NurseViewModelMappings : Profile
    {
        public NurseViewModelMappings()
        {
            CreateMap<NurseViewModel, AddNurseRequest>();
            CreateMap<AddNurseResponse, NurseViewModel>();
            CreateMap<GetNursesFromDepartamentResponse, NurseViewModel>();
            CreateMap<GetNurseResponse, NurseViewModel>();
            CreateMap<NurseViewModel, EditNurseRequest>();
        }
    }
}
