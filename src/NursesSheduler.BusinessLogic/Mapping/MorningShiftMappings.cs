using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Commands.CalculateMorningShifts;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Commands.UpsertMorningShifts;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.GetMorningShifts;
using NursesScheduler.BusinessLogic.CommandsAndQueries.NurseStats.Commands.RecalculateNurseStats;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.GetScheduleStatsFromSchedule;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class MorningShiftMappings : Profile
    {
        public MorningShiftMappings()
        {
            CreateMap<MorningShift, GetMorningShiftsResponse>();

            CreateMap<MorningShift, CalculateMorningShiftsResponse>();

            CreateMap<UpsertMorningShiftsRequest.MorningShiftRequest, MorningShift>();
            CreateMap<MorningShift, UpsertMorningShiftsResponse>();

            CreateMap<RecalculateNurseStatsRequest.MorningShiftRequest, MorningShift>();

            CreateMap<GetScheduleStatsFromScheduleRequest.MorningShiftRequest, MorningShift>();
        }
    }
}
