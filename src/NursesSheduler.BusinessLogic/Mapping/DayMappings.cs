using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Days.Queries.GetMonthDays;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.RecalculateScheduleHours;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class DayMappings : Profile
    {
        public DayMappings()
        {
            CreateMap<Day, GetMonthDaysResponse>();

            CreateMap<RecalculateScheduleHoursRequest.DayRequest, Day>();
        }
    }
}
