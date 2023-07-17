using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNursesScheduleStats;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleNurseMappings : Profile
    {
        public ScheduleNurseMappings()
        {
            CreateMap<ScheduleNurse, GetScheduleResponse.ScheduleNurseResponse>();
            CreateMap<RecalculateNurseScheduleStatsRequest.ScheduleNurseRequest, ScheduleNurse>();
            CreateMap<GetScheduleStatsRequest.ScheduleNurseRequest, ScheduleNurse>();
        }
    }
}
