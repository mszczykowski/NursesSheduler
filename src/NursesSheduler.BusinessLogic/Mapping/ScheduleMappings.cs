using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Commands.CreateSchedule;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleMappings : Profile
    {
        public ScheduleMappings()
        {
            CreateMap<Schedule, GetScheduleResponse>();
            CreateMap<Schedule, CreateScheduleResponse>();
        }
    }
}
