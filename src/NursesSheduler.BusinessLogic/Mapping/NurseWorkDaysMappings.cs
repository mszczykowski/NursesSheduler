using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.CloseSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.BuildSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.SolveSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;
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

            CreateMap<RecalculateNurseStatsRequest.NurseWorkDayRequest, NurseWorkDay>();
            
            CreateMap<GetScheduleStatsFromScheduleRequest.NurseWorkDayRequest, NurseWorkDay>();

            CreateMap<UpsertScheduleRequest.NurseWorkDayRequest, NurseWorkDay>();
            CreateMap<NurseWorkDay, UpsertScheduleResponse.NurseWorkDayResponse>();

            CreateMap<SolveScheduleRequest.NurseWorkDayRequest, NurseWorkDay>();
            CreateMap<NurseWorkDay, SolveScheduleResponse.NurseWorkDayResponse>();

            CreateMap<CloseScheduleRequest.NurseWorkDayRequest, NurseWorkDay>();
            CreateMap<NurseWorkDay, CloseScheduleResponse.NurseWorkDayResponse>();
        }
    }
}
