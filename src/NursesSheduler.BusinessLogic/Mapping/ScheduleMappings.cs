using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNursesScheduleStats;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleMappings : Profile
    {
        public ScheduleMappings()
        {
            CreateMap<Schedule, GetScheduleResponse>();

            CreateMap<GetScheduleStatsRequest.ScheduleRequest, Schedule>();
            CreateMap<RecalculateNurseScheduleStatsRequest.ScheduleRequest, Schedule>();
        }
    }
}
