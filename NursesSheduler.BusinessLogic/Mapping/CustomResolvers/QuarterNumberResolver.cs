using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping.CustomResolvers
{
    internal sealed class QuarterNumberResolver : IValueResolver<Schedule, GetScheduleResponse, int>
    {
        public int Resolve(Schedule source, GetScheduleResponse destination, int destMember,
                                ResolutionContext context)
        {
            return source.Quarter.QuarterNumber;
        }
    }
}
