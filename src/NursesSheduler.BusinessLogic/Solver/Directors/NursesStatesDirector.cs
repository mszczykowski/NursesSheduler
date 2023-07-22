using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Services;
using NursesScheduler.BusinessLogic.Solver.Builders;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Solver.Directors
{
    internal class NursesStatesDirector
    {
        public IEnumerable<INurseState> BuildNurseStats(Schedule currentSchedule, QuarterStats quarterStats, 
            ScheduleStats previousScheduleStats, ScheduleStats currentScheduleStats, ScheduleStats nextScheduleStats,
            WorkTimeService workTimeService, IEnumerable<Nurse> nurses)
        {
            var nurseStates = new List<INurseState>();

            var nurseStateBuilder = new NurseStateBuilder();

            foreach(var scheduleNurse in currentSchedule.ScheduleNurses)
            {
                nurseStates.Add(nurseStateBuilder
                    .SetNurseId(scheduleNurse)
                    .SetWorkTimeAssignedInWeeks(quarterStats.NurseStats.First(n => n.NurseId == scheduleNurse.NurseId))
                    .SetHoursFromLastShift(DateTime.DaysInMonth(previousScheduleStats.Key.Year, 
                        previousScheduleStats.Key.Month), 
                        previousScheduleStats.NursesScheduleStats.First(n => n.NurseId == scheduleNurse.NurseId))
                    .SetHoursToNextShiftMatrix(DateTime.DaysInMonth(nextScheduleStats.Key.Year,
                        nextScheduleStats.Key.Month), scheduleNurse.NurseWorkDays, 
                        nextScheduleStats.NursesScheduleStats.FirstOrDefault(n => n.NurseId == scheduleNurse.NurseId),
                        workTimeService)
                    .SetNumbersOfShifts(currentScheduleStats.WorkTimeInMonth, 
                        currentScheduleStats.NursesScheduleStats.First(n => n.NurseId == scheduleNurse.NurseId))
                    .SetSpecialWorkHours(nurses.First(n => n.NurseId == scheduleNurse.NurseId))
                    .GetResult());
            }
        }
    }
}
