using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Commands;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class MorningShiftMappings : Profile
    {
        public MorningShiftMappings()
        {
            CreateMap<MorningShift, GetScheduleResponse.MorningShiftsResponse>();
            CreateMap<MorningShift, CalculateMorningShiftsResponse>();
        }
    }
}
