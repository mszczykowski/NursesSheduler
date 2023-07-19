using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.BuildSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromScheduleQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class ScheduleNurseViewModelMappings : Profile
    {
        public ScheduleNurseViewModelMappings()
        {
            CreateMap<BuildScheduleResponse.ScheduleNurseResponse, ScheduleNurseViewModel>();

            CreateMap<GetScheduleResponse.ScheduleNurseResponse, ScheduleNurseViewModel>();

            CreateMap<ScheduleNurseViewModel, GetScheduleStatsFromScheduleRequest.ScheduleNurseRequest>();

            CreateMap<ScheduleNurseViewModel, RecalculateNurseScheduleStatsRequest.ScheduleNurseRequest>();
        }
    }
}
