using AutoMapper;
using NursesScheduler.BlazorShared.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class ScheduleNurseViewModelMappings : Profile
    {
        public ScheduleNurseViewModelMappings()
        {
            CreateMap<GetScheduleResponse.ScheduleNurseResponse, ScheduleNurseViewModel>();

            CreateMap<ScheduleNurseViewModel, GetScheduleStatsRequest.ScheduleNurseRequest>();
        }
    }
}
