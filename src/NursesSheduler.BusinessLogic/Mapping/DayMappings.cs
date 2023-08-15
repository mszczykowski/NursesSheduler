using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Days.Queries.GetMonthDays;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class DayMappings : Profile
    {
        public DayMappings()
        {
            CreateMap<DayNumbered, GetMonthDaysResponse.DayResponse>();
        }
    }
}
