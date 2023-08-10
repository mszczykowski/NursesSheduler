using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Abstractions.Services
{
    internal interface IScheduleValidatorService
    {
        IEnumerable<ScheduleValidationResult> ValidateScheduleNurse(TimeSpan maxWorkTimeInQuarter,
            ScheduleNurse scheduleNurse, NurseStats nurseQuarterStats, DepartamentSettings departamentSettings);
    }
}