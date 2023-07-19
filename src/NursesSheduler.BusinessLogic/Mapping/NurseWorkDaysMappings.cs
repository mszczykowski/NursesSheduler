using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class NurseWorkDaysMappings : Profile
    {
        public NurseWorkDaysMappings()
        {
            CreateMap<NurseWorkDay, BuildScheduleResponse.NurseWorkDayResponse>();

            CreateMap<RecalculateNurseScheduleStatsRequest.NurseWorkDayRequest, NurseWorkDay>();
            
            CreateMap<GetScheduleStatsFromScheduleRequest.NurseWorkDayRequest, NurseWorkDay>();
        }
    }
}
