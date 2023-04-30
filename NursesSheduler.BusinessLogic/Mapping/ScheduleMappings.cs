using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.BusinessLogic.Mapping.CustomResolvers;
using NursesScheduler.Domain.DomainModels;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleMappings : Profile
    {
        public ScheduleMappings()
        {
            CreateMap<Schedule, GetScheduleResponse>()
                .ForMember(dest => dest.QuarterNumber, opt => opt.MapFrom<QuarterNumberResolver>())
                .ForMember(dest => dest.WorkTimeInQuarter, opt => opt.MapFrom<WorkTimeInQuarterResolver>());
        }
    }
}
