using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class NurseWorkDayViewModelMappings : Profile
    {
        public NurseWorkDayViewModelMappings()
        {
            CreateMap<GetScheduleResponse.NurseWorkDayResponse, NurseWorkDayViewModel>();

            CreateMap<NurseWorkDayViewModel, GetScheduleStatsRequest.NurseWorkDayRequest>();
        }
    }
}
