using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleNurseMappings : Profile
    {
        public ScheduleNurseMappings()
        {
            CreateMap<ScheduleNurse, BuildScheduleResponse.ScheduleNurseResponse>();
            CreateMap<RecalculateNurseScheduleStatsRequest.ScheduleNurseRequest, ScheduleNurse>();
            CreateMap<GetScheduleStatsFromScheduleRequest.ScheduleNurseRequest, ScheduleNurse>();
        }
    }
}
