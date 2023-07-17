using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsQuery;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNursesScheduleStats;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class NurseWorkDaysMappings : Profile
    {
        public NurseWorkDaysMappings()
        {
            CreateMap<NurseWorkDay, GetScheduleResponse.NurseWorkDayResponse>();

            CreateMap<RecalculateNurseScheduleStatsRequest.NurseWorkDayRequest, NurseWorkDay>();
            
            CreateMap<GetScheduleStatsRequest.NurseWorkDayRequest, NurseWorkDay>();
        }
    }
}
