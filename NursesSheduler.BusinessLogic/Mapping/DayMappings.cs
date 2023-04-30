using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.Domain.Models;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class DayMappings : Profile
    {
        public DayMappings()
        {
            CreateMap<Day, GetScheduleResponse.DayResponse>();
        }
    }
}
