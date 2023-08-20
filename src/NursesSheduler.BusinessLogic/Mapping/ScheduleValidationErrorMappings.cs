using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.SchedulesStats.Queries.RecalculateNurseScheduleStats;
using NursesScheduler.Domain.ValueObjects;

namespace NursesScheduler.BusinessLogic.Mapping
{
    internal class ScheduleValidationErrorMappings : Profile
    {
        public ScheduleValidationErrorMappings()
        {
            CreateMap<ScheduleValidationError, RecalculateNurseStatsResponse.ScheduleValidationErrorResponse>();
        }
    }
}
