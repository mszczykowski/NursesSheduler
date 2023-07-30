using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.UpsertSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.BuildSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.SolveSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class NurseWorkDayViewModelMappings : Profile
    {
        public NurseWorkDayViewModelMappings()
        {
            CreateMap<GetScheduleResponse.NurseWorkDayResponse, NurseWorkDayViewModel>();

            CreateMap<BuildScheduleResponse.NurseWorkDayResponse, NurseWorkDayViewModel>();

            CreateMap<NurseWorkDayViewModel, GetScheduleStatsFromScheduleRequest.NurseWorkDayRequest>();

            CreateMap<NurseWorkDayViewModel, RecalculateNurseScheduleStatsRequest.NurseWorkDayRequest>();

            CreateMap<NurseWorkDayViewModel, SolveScheduleRequest.NurseWorkDayRequest>();
            CreateMap<SolveScheduleResponse.NurseWorkDayResponse , NurseWorkDayViewModel>();

            CreateMap<NurseWorkDayViewModel, UpsertScheduleRequest.NurseWorkDayRequest>();
            CreateMap<UpsertScheduleResponse.NurseWorkDayResponse, NurseWorkDayViewModel>();
        }
    }
}
