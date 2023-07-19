using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.BuildSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromScheduleQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class NurseWorkDaysMappings : Profile
    {
        public NurseWorkDaysMappings()
        {
            CreateMap<NurseWorkDay, BuildScheduleResponse.NurseWorkDayResponse>();

            CreateMap<NurseWorkDay, GetScheduleResponse.NurseWorkDayResponse>();

            CreateMap<RecalculateNurseScheduleStatsRequest.NurseWorkDayRequest, NurseWorkDay>();
            
            CreateMap<GetScheduleStatsFromScheduleRequest.NurseWorkDayRequest, NurseWorkDay>();
        }
    }
}
