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
    internal sealed class NurseWorkDayViewModelMappings : Profile
    {
        public NurseWorkDayViewModelMappings()
        {
            CreateMap<GetScheduleResponse.NurseWorkDayResponse, NurseWorkDayViewModel>();

            CreateMap<BuildScheduleResponse.NurseWorkDayResponse, NurseWorkDayViewModel>();

            CreateMap<NurseWorkDayViewModel, GetScheduleStatsFromScheduleRequest.NurseWorkDayRequest>();

            CreateMap<NurseWorkDayViewModel, RecalculateNurseStatsRequest.NurseWorkDayRequest>();

            CreateMap<NurseWorkDayViewModel, SolveScheduleRequest.NurseWorkDayRequest>();
            CreateMap<SolveScheduleResponse.NurseWorkDayResponse , NurseWorkDayViewModel>();

            CreateMap<NurseWorkDayViewModel, UpsertScheduleRequest.NurseWorkDayRequest>();
            CreateMap<UpsertScheduleResponse.NurseWorkDayResponse, NurseWorkDayViewModel>();

            CreateMap<NurseWorkDayViewModel, CloseScheduleRequest.NurseWorkDayRequest>();
        }
    }
}
