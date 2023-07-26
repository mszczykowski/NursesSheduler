using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.BuildSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromScheduleQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleNurseMappings : Profile
    {
        public ScheduleNurseMappings()
        {
            CreateMap<ScheduleNurse, BuildScheduleResponse.ScheduleNurseResponse>();

            CreateMap<ScheduleNurse, GetScheduleResponse.ScheduleNurseResponse>();

            CreateMap<RecalculateNurseScheduleStatsRequest.ScheduleNurseRequest, ScheduleNurse>();

            CreateMap<GetScheduleStatsFromScheduleRequest.ScheduleNurseRequest, ScheduleNurse>();

            CreateMap<UpsertScheduleRequest.ScheduleNurseRequest, ScheduleNurse>();
        }
    }
}
