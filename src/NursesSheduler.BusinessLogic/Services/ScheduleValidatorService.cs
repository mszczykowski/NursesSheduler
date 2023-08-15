using NursesScheduler.BusinessLogic.Abstractions.Infrastructure.Providers;
using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Services
{
    internal sealed class ScheduleValidatorService : IScheduleValidatorService
    {
        private readonly IDepartamentSettingsProvider _departamentSettingsProvider;
        private readonly IWorkTimeService _workTimeService;

        public ScheduleValidatorService(IDepartamentSettingsProvider departamentSettingsProvider, 
            IWorkTimeService workTimeService)
        {
            _departamentSettingsProvider = departamentSettingsProvider;
            _workTimeService = workTimeService;
        }

        public async Task<IEnumerable<ScheduleValidationError>> ValidateScheduleNurse(TimeSpan maxWorkTimeInQuarter,
            ScheduleNurse scheduleNurse, NurseStats nurseQuarterStats, NurseScheduleStats? previousScheduleNurseStats, 
            int departamentId)
        {
            var departamentSettings = await _departamentSettingsProvider.GetCachedDataAsync(departamentId);

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

                if(workDay.Day == 1 && previousScheduleNurseStats is null)
                {
                    continue;
                }

                var hoursFromLastShift = _workTimeService.GetHoursFromLastAssignedShift(workDay.Day, scheduleNurse.NurseWorkDays);
                if(workDay.ShiftType == Domain.Enums.ShiftTypes.Night)
                {
                    hoursFromLastShift += ScheduleConstatns.RegularShiftLength;
                }

                if ((workDay.Day == 1 &&
                    hoursFromLastShift
                    + previousScheduleNurseStats.HoursFromLastAssignedShift <
                    departamentSettings.MinimalShiftBreak)
                    ||
                    workDay.Day != 1 && hoursFromLastShift <
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
