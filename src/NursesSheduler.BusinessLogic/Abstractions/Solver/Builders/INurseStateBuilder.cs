using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Builders
{
    internal interface INurseStateBuilder : IBuilder<INurseState>
    {
        INurseStateBuilder BuildAssignedShifts(IEnumerable<NurseWorkDay> nurseWorkDays);
        INurseState GetResult();
        INurseStateBuilder SetAssignedMorningShift(IEnumerable<NurseWorkDay> nurseWorkDays);
        INurseStateBuilder SetHadNumberOfShiftsReduced(NurseQuarterStats nurseQuarterStats);
        INurseStateBuilder SetNextMonthHoursToNextShiftx(int nextMonthLength, NurseScheduleStats? nextScheduleNurseStats);
        INurseStateBuilder SetNumbersOfShifts(int numberOfShiftsToAssignInMonth, NurseScheduleStats nurseScheduleStats, 
            IEnumerable<NurseWorkDay> nurseWorkDays);
        INurseStateBuilder SetNurseId(ScheduleNurse scheduleNurse);
        INurseStateBuilder SetNurseTeam(Nurse nurse);
        INurseStateBuilder SetPreviouslyAssignedMorningShifts(NurseStats nurseQuarterStats);
        INurseStateBuilder SetPreviousMonthHoursFromLastShift(int previousMonthLength, 
            NurseScheduleStats? previousScheduleNurseStats);
        INurseStateBuilder SetPreviousMonthLastShift(NurseScheduleStats? previousScheduleNurseStats);
        INurseStateBuilder SetSpecialWorkHours(Nurse nurse, IEnumerable<ScheduleStats> schedulesStats);
        INurseStateBuilder SetTimeOffs(IEnumerable<NurseWorkDay> nurseWorkDays);
        INurseStateBuilder SetWorkTimeAssignedInWeeks(NurseStats nurseQuarterStats);
        INurseStateBuilder SetWorkTimeInQuarterLeft(TimeSpan workTimeInQuarter, NurseStats nurseQuarterStats);
    }
}
