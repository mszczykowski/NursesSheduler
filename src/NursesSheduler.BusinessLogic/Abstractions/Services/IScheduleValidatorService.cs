using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IScheduleValidatorService
    {
        Task<IEnumerable<ScheduleValidationError>> ValidateScheduleNurse(TimeSpan totalWorkTimeInQuarter,
            ScheduleNurse scheduleNurse, NurseStats nurseQuarterStats, NurseScheduleStats? previousScheduleNurseStats, 
            int departamentId);
    }
}