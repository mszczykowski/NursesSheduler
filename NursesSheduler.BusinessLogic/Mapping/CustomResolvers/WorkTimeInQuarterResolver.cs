using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.Schedules.Queries.GetSchedule;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Mapping.CustomResolvers
{
    internal sealed class WorkTimeInQuarterResolver : IValueResolver<Schedule, GetScheduleResponse, TimeSpan>
    {
        public TimeSpan Resolve(Schedule source, GetScheduleResponse destination, TimeSpan destMember,
                                ResolutionContext context)
        {
            return source.Quarter.WorkTimeInQuarterToAssign;
        }
    }
}
