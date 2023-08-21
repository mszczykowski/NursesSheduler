using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver.Directors;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Builders;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Solver.Directors
{
    internal class NursesStatesDirector : INursesStatesDirector
    {
        public IEnumerable<INurseState> BuildNurseStats(Schedule currentSchedule, QuarterStats quarterStats,
            ScheduleStats previousScheduleStats, ScheduleStats currentScheduleStats, ScheduleStats nextScheduleStats,
            IWorkTimeService workTimeService, IEnumerable<Nurse> nurses)
        {
            var nurseStates = new List<INurseState>();

            var nurseStateBuilder = new NurseStateBuilder();

            var scheduleStatsList = new List<ScheduleStats>()
                        {
                            previousScheduleStats,
                            currentScheduleStats,
                            nextScheduleStats,
                        };

            foreach (var scheduleNurse in currentSchedule.ScheduleNurses)
            {
                var nurse = nurses
                    .First(n => n.NurseId == scheduleNurse.NurseId);

                var nurseQuarterStats = quarterStats.NurseStats
                    .First(n => n.NurseId == scheduleNurse.NurseId);

                var previousScheduleNurseStats = previousScheduleStats.NursesScheduleStats
                    .FirstOrDefault(n => n.NurseId == scheduleNurse.NurseId);

                nurseStates.Add(nurseStateBuilder
                    .SetNurseId(scheduleNurse)
                    .SetWorkTimeAssignedInWeeks(nurseQuarterStats)
                    .SetHoursFromLastShift(DateTime.DaysInMonth(previousScheduleStats.Key.Year,
                        previousScheduleStats.Key.Month),
                        previousScheduleNurseStats)
                    .SetHoursToNextShiftMatrix(DateTime.DaysInMonth(nextScheduleStats.Key.Year,
                        nextScheduleStats.Key.Month), scheduleNurse.NurseWorkDays,
                        nextScheduleStats.NursesScheduleStats.FirstOrDefault(n => n.NurseId == scheduleNurse.NurseId),
                        workTimeService)
                    .SetNumbersOfShifts(quarterStats.ShiftsToAssignInMonths[currentScheduleStats.MonthInQuarter - 1], 
                        currentScheduleStats.NursesScheduleStats.First(n => n.NurseId == scheduleNurse.NurseId),
                        scheduleNurse.NurseWorkDays)
                    .SetSpecialWorkHours(nurse, scheduleStatsList)
                    .SetWorkTimeInQuarterLeft(quarterStats.WorkTimeInQuarter, nurseQuarterStats)
                    .SetTimeOffs(scheduleNurse.NurseWorkDays)
                    .SetPreviouslyAssignedMorningShifts(nurseQuarterStats)
                    .SetAssignedMorningShift(scheduleNurse.NurseWorkDays)
                    .SetPreviousMonthLastShift(previousScheduleNurseStats)
                    .SetNurseTeam(nurse)
                    .GetResult());
            }

            return nurseStates;
        }
    }
}
