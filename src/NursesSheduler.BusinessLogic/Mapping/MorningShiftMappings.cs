using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.CalculateMorningShifts;
using NursesScheduler.BusinessLogic.CommandsAndQueries.MorningShifts.Queries.GetMorningShifts;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal sealed class MorningShiftMappings : Profile
    {
        public MorningShiftMappings()
        {
            CreateMap<MorningShift, GetMorningShiftsResponse>();
            CreateMap<MorningShift, CalculateMorningShiftsResponse>();

        }
    }
}
