using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.NurseStats.Commands.RecalculateNurseStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.BuildSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.CloseSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.SolveSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class ScheduleNurseMappings : Profile
    {
        public ScheduleNurseMappings()
        {
            CreateMap<ScheduleNurse, BuildScheduleResponse.ScheduleNurseResponse>();

            CreateMap<ScheduleNurse, GetScheduleResponse.ScheduleNurseResponse>();

            CreateMap<RecalculateNurseStatsRequest.ScheduleNurseRequest, ScheduleNurse>();

            CreateMap<GetScheduleStatsFromScheduleRequest.ScheduleNurseRequest, ScheduleNurse>();

            CreateMap<UpsertScheduleRequest.ScheduleNurseRequest, ScheduleNurse>();
            CreateMap<ScheduleNurse, UpsertScheduleResponse.ScheduleNurseResponse>();

            CreateMap<SolveScheduleRequest.ScheduleNurseRequest, ScheduleNurse>();
            CreateMap<ScheduleNurse, SolveScheduleResponse.ScheduleNurseResponse>();

            CreateMap<CloseScheduleRequest.ScheduleNurseRequest, ScheduleNurse>();
            CreateMap<ScheduleNurse, CloseScheduleResponse.ScheduleNurseResponse>();
        }
    }
}
