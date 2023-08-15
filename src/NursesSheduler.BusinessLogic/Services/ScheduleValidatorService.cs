using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class ScheduleValidatorService : IScheduleValidatorService
    {
        private readonly IWorkTimeService _workTimeService;

        public ScheduleValidatorService(IWorkTimeService workTimeService)
        {
            _workTimeService = workTimeService;
        }

        public IEnumerable<ScheduleValidationError> ValidateScheduleNurse(TimeSpan maxWorkTimeInQuarter,
            ScheduleNurse scheduleNurse, NurseStats nurseQuarterStats, DepartamentSettings departamentSettings)
        {
            var validationResult = new List<ScheduleValidationError>();

            if (nurseQuarterStats.AssignedWorkTime > maxWorkTimeInQuarter)
            {
                validationResult.Add(new ScheduleValidationError
                {
                    Reason = Domain.Enums.ScheduleInvalidReasons.TooMuchHoursInQuarter,
                    AdditionalInfo = Math.Floor(nurseQuarterStats.AssignedWorkTime.TotalHours).ToString().PadLeft(2, '0') + ":" +
                                                            nurseQuarterStats.AssignedWorkTime.Minutes.ToString().PadLeft(2, '0')
                                                            .Replace("-", string.Empty),
                });
            }

            foreach (var weekNumber in nurseQuarterStats.WorkTimeAssignedInWeeks.Keys)
            {
                if (nurseQuarterStats.WorkTimeAssignedInWeeks[weekNumber] > departamentSettings.MaximumWeekWorkTimeLength)
                {
                    validationResult.Add(new ScheduleValidationError
                    {
                        Reason = Domain.Enums.ScheduleInvalidReasons.TooMuchHoursInWeek,
                        AdditionalInfo = weekNumber.ToString(),
                    });
                }
            }

            foreach (var workDay in scheduleNurse.NurseWorkDays)
            {
                if (workDay.ShiftType == Domain.Enums.ShiftTypes.None)
                {
                    continue;
                }

                if (_workTimeService.GetHoursFromLastAssignedShift(workDay.Day, scheduleNurse.NurseWorkDays) >
                    departamentSettings.MinimalShiftBreak)
                {
                    validationResult.Add(new ScheduleValidationError
                    {
                        Reason = Domain.Enums.ScheduleInvalidReasons.BreakBetweenShiftsTooShort,
                        AdditionalInfo = workDay.Day.ToString(),
                    });
                }
            }

            return validationResult;
        }
    }
}
