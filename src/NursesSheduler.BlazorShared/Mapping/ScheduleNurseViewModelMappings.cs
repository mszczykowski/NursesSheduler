using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.CloseSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.BuildSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.SolveSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class ScheduleNurseViewModelMappings : Profile
    {
        public ScheduleNurseViewModelMappings()
        {
            CreateMap<BuildScheduleResponse.ScheduleNurseResponse, ScheduleNurseViewModel>();

            CreateMap<GetScheduleResponse.ScheduleNurseResponse, ScheduleNurseViewModel>();

            CreateMap<ScheduleNurseViewModel, GetScheduleStatsFromScheduleRequest.ScheduleNurseRequest>();

            CreateMap<ScheduleNurseViewModel, RecalculateNurseScheduleStatsRequest.ScheduleNurseRequest>();

            CreateMap<ScheduleNurseViewModel, SolveScheduleRequest.ScheduleNurseRequest>();
            CreateMap<SolveScheduleResponse.ScheduleNurseResponse, ScheduleNurseViewModel>();

            CreateMap<ScheduleNurseViewModel, UpsertScheduleRequest.ScheduleNurseRequest>();
            CreateMap<UpsertScheduleResponse.ScheduleNurseResponse, ScheduleNurseViewModel>();

            CreateMap<ScheduleNurseViewModel, CloseScheduleRequest.ScheduleNurseRequest>();
        }
    }
}
