﻿using NursesScheduler.BusinessLogic.Abstractions.Services;
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

                var nextScheduleNurseStats = nextScheduleStats.NursesScheduleStats
                    .FirstOrDefault(n => n.NurseId == scheduleNurse.NurseId);

                nurseStates.Add(nurseStateBuilder
                    .SetNurseId(scheduleNurse)
                    .SetWorkTimeAssignedInWeeks(nurseQuarterStats)
                    .SetNextMonthHoursToNextShiftx(DateTime.DaysInMonth(previousScheduleStats.Key.Year,
                        previousScheduleStats.Key.Month),
                        nextScheduleNurseStats)
                    .SetPreviousMonthHoursFromLastShift(DateTime.DaysInMonth(nextScheduleStats.Key.Year,
                        nextScheduleStats.Key.Month),
                        previousScheduleNurseStats)
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
                    .SetHadNumberOfShiftsReduced(nurseQuarterStats)
                    .BuildAssignedShifts(scheduleNurse.NurseWorkDays)
                    .GetResult());
            }

            return nurseStates;
        }
    }
}
