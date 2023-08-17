using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Commands.UpsertMorningShifts;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.CalculateMorningShifts;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.GetMorningShifts;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;

namespace NursesScheduler.BlazorShared.Mapping
{
    internal sealed class MorningShiftViewModelMappings : Profile
    {
        public MorningShiftViewModelMappings()
        {
            CreateMap<GetMorningShiftsResponse, MorningShiftViewModel>();

            CreateMap<CalculateMorningShiftsResponse, MorningShiftViewModel>();

            CreateMap<MorningShiftViewModel, UpsertMorningShiftsRequest.MorningShiftRequest>();
            CreateMap<UpsertMorningShiftsResponse, MorningShiftViewModel>();

            CreateMap<MorningShiftViewModel, RecalculateNurseStatsRequest.MorningShiftRequest>();

            CreateMap<MorningShiftViewModel, GetScheduleStatsFromScheduleRequest.MorningShiftRequest>();
        }
    }
}
