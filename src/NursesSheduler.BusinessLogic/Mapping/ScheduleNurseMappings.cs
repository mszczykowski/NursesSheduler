using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.CloseSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.BuildSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.SolveSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.ScheduleValidations.Queries.ValidateScheduleNurse;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class ScheduleNurseMappings : Profile
    {
        public ScheduleNurseMappings()
        {
            CreateMap<ScheduleNurse, BuildScheduleResponse.ScheduleNurseResponse>();

            CreateMap<ScheduleNurse, GetScheduleResponse.ScheduleNurseResponse>();

            CreateMap<RecalculateNurseScheduleStatsRequest.ScheduleNurseRequest, ScheduleNurse>();

            CreateMap<GetScheduleStatsFromScheduleRequest.ScheduleNurseRequest, ScheduleNurse>();

            CreateMap<UpsertScheduleRequest.ScheduleNurseRequest, ScheduleNurse>();
            CreateMap<ScheduleNurse, UpsertScheduleResponse.ScheduleNurseResponse>();

            CreateMap<SolveScheduleRequest.ScheduleNurseRequest, ScheduleNurse>();
            CreateMap<ScheduleNurse, SolveScheduleResponse.ScheduleNurseResponse>();

            CreateMap<ValidateScheduleNurseRequest, ScheduleNurse>();

            CreateMap<CloseScheduleRequest.ScheduleNurseRequest, ScheduleNurse>();
            CreateMap<ScheduleNurse, CloseScheduleResponse.ScheduleNurseResponse>();
        }
    }
}
