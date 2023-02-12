using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.Domain.DatabaseModels.Schedules;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleMappings : Profile
    {
        public ScheduleMappings()
        {
            CreateMap<Schedule, GetScheduleResponse>()
                .ReverseMap();
        }
    }
}
