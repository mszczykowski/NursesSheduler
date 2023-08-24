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
        public int CurrentDay { get; private set; }
        public ShiftIndex CurrentShift { get; private set; }

        public int NursesToAssignForCurrentShift { get; private set; }
        public int NursesToAssignForMorningShift { get; private set; }

        public ICollection<INurseState> NurseStates { get; }
        public IDictionary<int, ShiftTypes[]> ScheduleState { get; }
        public bool IsShiftAssigned => NursesToAssignForCurrentShift == 0 && NursesToAssignForMorningShift == 0;

        public SolverState(Schedule schedule, int monthLength, IEnumerable<INurseState> nurses)
        {
            CurrentDay = 1;
            CurrentShift = ShiftIndex.Day;

            NurseStates = new List<INurseState>();
            foreach (var nurse in nurses)
            {
                NurseStates.Add(new NurseState(nurse));
            }

            ScheduleState = new Dictionary<int, ShiftTypes[]>();
            foreach (var nurse in schedule.ScheduleNurses)
            {
                ScheduleState.Add(nurse.NurseId, new ShiftTypes[monthLength]);

                foreach (var workday in nurse.NurseWorkDays)
                {
                    ScheduleState[nurse.NurseId][workday.Day - 1] = workday.ShiftType;
                }
            }

            SetHoursFromLastShift();
        }

        public SolverState(ISolverState stateToCopy)
        {
            //shallow copies
            CurrentDay = stateToCopy.CurrentDay;
            CurrentShift = stateToCopy.CurrentShift;

            NursesToAssignForCurrentShift = stateToCopy.NursesToAssignForCurrentShift;
            NursesToAssignForMorningShift = stateToCopy.NursesToAssignForMorningShift;

            //deep copies
            NurseStates = new List<INurseState>(stateToCopy.NurseStates.Select(s => new NurseState(s)).ToList());

            ScheduleState = stateToCopy.ScheduleState
                .ToDictionary(entry => entry.Key, entry => (ShiftTypes[])entry.Value.Clone());
        }

        public void AssignNurseToRegularShift(INurseState nurse)
        {
            NursesToAssignForCurrentShift--;
            AssignNurseToCurrentShift(nurse.NurseId);
        }

        public void AssignNurseToMorningShift(INurseState nurse, bool swapRegularForMorning)
        {
            NursesToAssignForMorningShift--;

            if(swapRegularForMorning)
            {
                NursesToAssignForCurrentShift--;
            }

            AssignNurseToShift(nurse.NurseId, ShiftTypes.Morning);
        }

        public void AssignNurseOnTimeOff(INurseState nurse)
        {
            AssignNurseToCurrentShift(nurse.NurseId);
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
                .Where(n => ScheduleState[n.NurseId][CurrentDay - 1] == ShiftTypes.None))
            {
                nurseState.AdvanceState();
            }
        }

        public void SetNursesToAssignCounts(IShiftCapacityManager shiftCapacityManager)
        {
            NursesToAssignForCurrentShift = shiftCapacityManager
                .GetNumberOfNursesForRegularShift(CurrentShift, CurrentDay);
            NursesToAssignForMorningShift = shiftCapacityManager
                .GetNumberOfNursesForMorningShift(CurrentShift, CurrentDay);
        }

        public HashSet<int> GetPreviousDayDayShift()
        {
            if (CurrentDay == 1)
            {
                return NurseStates
                    .Where(n => n.PreviousMonthLastShift == ShiftTypes.Day)
                    .Select(n => n.NurseId)
                    .ToHashSet();
            }

            return ScheduleState
                .Where(entry => entry.Value[CurrentDay - 2] == ShiftTypes.Day)
                    //|| entry.Value[CurrentDay - 2] == ShiftTypes.Morning)
                .Select(entry => entry.Key)
                .ToHashSet();
        }

        public void PopulateScheduleFromState(Schedule schedule)
        {
            foreach (var scheduleNurse in schedule.ScheduleNurses)
            {
                var nurseSchedule = ScheduleState[scheduleNurse.NurseId];
                foreach (var workDay in scheduleNurse.NurseWorkDays)
                {
                    if (workDay.ShiftType != ShiftTypes.None)
                    {
                        continue;
                    }

                    workDay.ShiftType = nurseSchedule[workDay.Day - 1];

                    if (workDay.ShiftType == ShiftTypes.Morning)
                    {
                        workDay.MorningShiftId = NurseStates
                            .First(n => n.NurseId == scheduleNurse.NurseId).AssignedMorningShiftId;
                    }
                }
            }
        }

        private void AssignNurseToCurrentShift(int nurseId)
        {
            if (CurrentShift == ShiftIndex.Day)
            {
                AssignNurseToShift(nurseId, ShiftTypes.Day);
            }
            else
            {
                AssignNurseToShift(nurseId, ShiftTypes.Night);
            }
        }

        private void AssignNurseToShift(int nurseId, ShiftTypes shiftType)
        {
            ScheduleState[nurseId][CurrentDay - 1] = shiftType;
        }

        public void SetHoursFromLastShift()
        {
            if(CurrentShift == ShiftIndex.Day)
            {
                foreach (var nurse in NurseStates.Where(n => ScheduleState[n.NurseId][CurrentDay - 1] == ShiftTypes.Day 
                    || ScheduleState[n.NurseId][CurrentDay - 1] == ShiftTypes.Morning))

                {
                    nurse.ResetHoursFromLastShift();
                }
            }
            else if (CurrentShift == ShiftIndex.Night)
            {
                foreach (var nurse in NurseStates.Where(n => ScheduleState[n.NurseId][CurrentDay - 1] == ShiftTypes.Night))
                {
                    nurse.ResetHoursFromLastShift();
                }
            }

        }
    }
}
