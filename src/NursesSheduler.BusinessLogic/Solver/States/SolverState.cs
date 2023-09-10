using NursesScheduler.BusinessLogic.Abstractions.Solver.Managers;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain.Constants;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Solver.States
{
    internal sealed class SolverState : ISolverState
    {
        public int CurrentDay { get; set; }
        public ShiftIndex CurrentShift { get; private set; }

        public int NursesToAssignForCurrentShift { get; set; }

        public ICollection<INurseState> NurseStates { get; }

        public bool IsShiftAssigned => NursesToAssignForCurrentShift == 0;

        public SolverState(IEnumerable<INurseState> nurses)
        {
            CurrentDay = 1;
            CurrentShift = ShiftIndex.Day;

            NurseStates = new List<INurseState>();
            foreach (var nurse in nurses)
            {
                NurseStates.Add(new NurseState(nurse));
            }

            SetHoursFromLastShift();
        }

        public SolverState(ISolverState stateToCopy)
        {
            //shallow copies
            CurrentDay = stateToCopy.CurrentDay;
            CurrentShift = stateToCopy.CurrentShift;

            NursesToAssignForCurrentShift = stateToCopy.NursesToAssignForCurrentShift;

            //deep copies
            NurseStates = new List<INurseState>(stateToCopy.NurseStates.Select(s => new NurseState(s)).ToList());
        }

        public void AdvanceShiftAndDay()
        {
            CurrentShift++;
            if ((int)CurrentShift >= ScheduleConstatns.NumberOfShifts)
            {
                CurrentShift = ShiftIndex.Day;
                CurrentDay++;
            }
        }

        public void AdvanceUnassignedNursesState()
        {
            foreach (var nurseState in NurseStates
                .Where(n => n.ScheduleRow[CurrentDay - 1] == ShiftTypes.None))
            {
                nurseState.AdvanceState();
            }
        }

        public void SetNursesToAssignCounts(IShiftCapacityManager shiftCapacityManager)
        {
            NursesToAssignForCurrentShift = shiftCapacityManager
                .GetNumberOfNursesForRegularShift(CurrentShift, CurrentDay);
        }

        public IEnumerable<int> GetPreviousDayDayShift()
        {
            if (CurrentDay == 1)
            {
                return NurseStates
                    .Where(n => n.PreviousMonthLastShift == ShiftTypes.Day)
                    .Select(n => n.NurseId);
            }

            return NurseStates
                .Where(n => n.ScheduleRow[CurrentDay - 2] == ShiftTypes.Day)
                .Select(n => n.NurseId);
        }

        public void PopulateScheduleFromState(Schedule schedule)
        {
            foreach (var scheduleNurse in schedule.ScheduleNurses)
            {
                var nurseState = NurseStates.First(n => n.NurseId == scheduleNurse.NurseId);

                foreach (var workDay in scheduleNurse.NurseWorkDays)
                {
                    if (workDay.ShiftType != ShiftTypes.None)
                    {
                        continue;
                    }

                    workDay.ShiftType = nurseState.ScheduleRow[workDay.Day - 1];

                    if (workDay.ShiftType == ShiftTypes.Morning)
                    {
                        workDay.MorningShiftId = nurseState.AssignedMorningShiftId;
                    }
                }
            }
        }

        public void SetHoursFromLastShift()
        {
            if (CurrentShift == ShiftIndex.Day)
            {
                foreach (var nurse in NurseStates.Where(n => n.ScheduleRow[CurrentDay - 1] == ShiftTypes.Day
                    || n.ScheduleRow[CurrentDay - 1] == ShiftTypes.Morning))

                {
                    nurse.ResetHoursFromLastShift();
                }
            }
            else if (CurrentShift == ShiftIndex.Night)
            {
                foreach (var nurse in NurseStates.Where(n => n.ScheduleRow[CurrentDay - 1] == ShiftTypes.Night))
                {
                    nurse.ResetHoursFromLastShift();
                }
            }

        }
    }
}
