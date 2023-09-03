using AutoMapper;
using NursesScheduler.BlazorShared.Models.ViewModels.Entities;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Commands.CalculateMorningShifts;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Commands.UpsertMorningShifts;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.GetMorningShifts;
using NursesScheduler.BusinessLogic.CommandsAndQueries.NurseStats.Commands.RecalculateNurseStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;

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
