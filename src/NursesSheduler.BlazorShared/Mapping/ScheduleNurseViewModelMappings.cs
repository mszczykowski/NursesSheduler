using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using NursesScheduler.BusinessLogic.CommandsAndQueries.NurseStats.Commands.RecalculateNurseStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.BuildSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.CloseSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.SolveSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class ScheduleNurseViewModelMappings : Profile
    {
        public ScheduleNurseViewModelMappings()
        {
            CreateMap<BuildScheduleResponse.ScheduleNurseResponse, ScheduleNurseViewModel>();

            CreateMap<GetScheduleResponse.ScheduleNurseResponse, ScheduleNurseViewModel>();

            CreateMap<ScheduleNurseViewModel, GetScheduleStatsFromScheduleRequest.ScheduleNurseRequest>();

            CreateMap<ScheduleNurseViewModel, RecalculateNurseStatsRequest.ScheduleNurseRequest>();

            CreateMap<ScheduleNurseViewModel, SolveScheduleRequest.ScheduleNurseRequest>();
            CreateMap<SolveScheduleResponse.ScheduleNurseResponse, ScheduleNurseViewModel>();

            CreateMap<ScheduleNurseViewModel, UpsertScheduleRequest.ScheduleNurseRequest>();
            CreateMap<UpsertScheduleResponse.ScheduleNurseResponse, ScheduleNurseViewModel>();

            CreateMap<ScheduleNurseViewModel, CloseScheduleRequest.ScheduleNurseRequest>();
        }
    }
}
