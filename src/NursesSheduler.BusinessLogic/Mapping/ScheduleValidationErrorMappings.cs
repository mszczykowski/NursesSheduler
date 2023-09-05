using AutoMapper;
using NursesScheduler.BusinessLogic.CommandsAndQueries.NurseStats.Commands.RecalculateNurseStats;
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
