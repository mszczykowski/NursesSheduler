using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Solver.StateManagers
{
    internal sealed class SolverState : ISolverState
    {
        public int CurrentDay { get; set; }
        public int DayNumberInQuarter { get; set; }
        public int WeekInQuarter => (int)Math.Ceiling((decimal)DayNumberInQuarter / 7) - 1;
        public ShiftIndex CurrentShift { get; set; }

        public int NursesToAssignForCurrentShift { get; set; }
        public int NursesToAssignForMorningShift { get; set; }
        public int NursesToAssignOnTimeOff { get; set; }

        public HashSet<INurseState> Nurses { get; }
        public HashSet<int>[,] ScheduleState { get; }
        public HashSet<int>[] MorningShiftsState { get; }

        public bool IsShiftAssined => NursesToAssignForCurrentShift <= 0 && NursesToAssignForMorningShift <= 0 && 
            NursesToAssignOnTimeOff == 0;

        private int _currentDayIndex => CurrentDay - 1;

        public SolverState(Schedule schedule, ICollection<INurseState> nurses)
        {
            CurrentDay = 1;
            CurrentShift = 0;

            NursesToAssignForCurrentShift = 0;
            NursesToAssignForMorningShift = 0;
            NursesToAssignOnTimeOff = 0;

            Nurses = new HashSet<INurseState>();
            foreach(var nurse in nurses)
            {
                Nurses.Add(new NurseState(nurse));
            }

            var numberOfDays = schedule.ScheduleNurses.First().NurseWorkDays.Max(d => d.DayNumber);

            ScheduleState = new HashSet<int>[numberOfDays, GeneralConstants.NumberOfShifts];
            MorningShiftsState = new HashSet<int>[numberOfDays];

            foreach (var nurse in schedule.ScheduleNurses)
            {
                foreach (var day in nurse.NurseWorkDays)
                {
                    if (day.ShiftType == ShiftTypes.Day)
                    {
                        AddNurseToRegularShift(nurse.NurseId, day.DayNumber, ShiftIndex.Day);
                    }
                    else if (day.ShiftType == ShiftTypes.Night)
                    {
                        AddNurseToRegularShift(nurse.NurseId, day.DayNumber, ShiftIndex.Night);
                    }
                    else if (day.ShiftType == ShiftTypes.Morning)
                    {
                        AddNurseToMorningShift(nurse.NurseId, day.DayNumber);
                    }
                }
            }
        }

        public SolverState(ISolverState stateToCopy)
        {
            CurrentDay = stateToCopy.CurrentDay;
            CurrentShift = stateToCopy.CurrentShift;

            Nurses = new HashSet<INurseState>();
            foreach (var nurse in stateToCopy.Nurses)
            {
                Nurses.Add(new NurseState(nurse));
            }

            ScheduleState = new HashSet<int>[stateToCopy.ScheduleState.GetLength(0),
                stateToCopy.ScheduleState.GetLength(1)];

            for (int i = 0; i < ScheduleState.GetLength(0); i++)
            {
                for (int j = 0; j < ScheduleState.GetLength(1); j++)
                {
                    if (stateToCopy.ScheduleState[i, j] != null)
                    {
                        ScheduleState[i, j] = new HashSet<int>(stateToCopy.ScheduleState[i, j]);
                    }
                }
            }

            MorningShiftsState = new HashSet<int>[stateToCopy.MorningShiftsState.Length];

            for (int i = 0; i < MorningShiftsState.Length; i++)
            {
                if (stateToCopy.MorningShiftsState[i] != null)
                {
                    MorningShiftsState[i] = new HashSet<int>(stateToCopy.MorningShiftsState[i]);
                }
            }

            NursesToAssignForCurrentShift = stateToCopy.NursesToAssignForCurrentShift;
            NursesToAssignForMorningShift = stateToCopy.NursesToAssignForMorningShift;
            NursesToAssignOnTimeOff = stateToCopy.NursesToAssignOnTimeOff;
        }

        private void AdvanceShiftAndDay()
        {
            CurrentShift++;
            if ((int)CurrentShift >= GeneralConstants.NumberOfShifts)
            {
                CurrentShift = 0;
                CurrentDay++;
                DayNumberInQuarter++;
            }

            if (_currentDayIndex < ScheduleState.GetLength(0))
            {
                foreach (var e in Nurses)
                {
                    e.AdvanceState(CalculateNurseHoursToNextShift(e.NurseId));
                }

                if(ScheduleState[_currentDayIndex, (int)CurrentShift] != null)
                {
                    foreach (var nurseId in ScheduleState[_currentDayIndex, (int)CurrentShift])
                    {
                        Nurses.First(n => n.NurseId == nurseId).ResetHoursFromLastShift();
                    }
                }
            }
        }

        public void AdvanceStateRegularShift()
        {
            NursesToAssignForCurrentShift--;

            if (IsShiftAssined)
            {
                AdvanceShiftAndDay();
            }
        }

        public void AdvanceStateMorningShift()
        {
            NursesToAssignForMorningShift--;

            if (IsShiftAssined)
            {
                AdvanceShiftAndDay();
            }
        }

        public void AdvanceStateTimeOffShift()
        {
            NursesToAssignOnTimeOff--;

            if (IsShiftAssined)
            {
                AdvanceShiftAndDay();
            }
        }

        public HashSet<int> GetPreviousDayShift()
        {
            if (CurrentDay == 1)
            {
                return Nurses.Where(n => n.PreviousMonthLastShift == ShiftIndex.Day).Select(n => n.NurseId).ToHashSet();
            }

            HashSet<int> result;

            if (CurrentShift == ShiftIndex.Day)
            {
                result = ScheduleState[_currentDayIndex - 1, (int)ShiftIndex.Day];
                if (MorningShiftsState[_currentDayIndex - 1] != null)
                {
                    result.UnionWith(MorningShiftsState[_currentDayIndex - 1]);
                }
            }
            else
            {
                result = ScheduleState[_currentDayIndex, (int)ShiftIndex.Day];
                if (MorningShiftsState[_currentDayIndex] != null)
                {
                    result.UnionWith(MorningShiftsState[_currentDayIndex]);
                }
            }

            return result;
        }

        public void AssignNurseToRegularShift(INurseState nurse, bool isHoliday,
            DepartamentSettings departamentSettings)
        {
            AddNurseToRegularShift(nurse.NurseId);

            nurse.UpdateStateOnRegularShiftAssign(isHoliday, CurrentShift, WeekInQuarter, departamentSettings,
                CalculateNurseHoursToNextShift(nurse.NurseId));
        }

        public void AssignNurseOnTimeOff(INurseState nurse, bool isHoliday,
            DepartamentSettings departamentSettings)
        {
            AddNurseToRegularShift(nurse.NurseId);

            nurse.UpdateStateOnTimeOffShiftAssign(isHoliday, CurrentShift, WeekInQuarter, departamentSettings,
                CalculateNurseHoursToNextShift(nurse.NurseId));
        }

        public void AssignEmployeeToMorningShift(INurseState nurse, MorningShift morningShift)
        {
            AddNurseToMorningShift(nurse.NurseId);

            nurse.UpdateStateOnMorningShiftAssign(morningShift, WeekInQuarter,
                CalculateNurseHoursToNextShift(nurse.NurseId));
        }

        public TimeSpan GetHoursToScheduleEnd()
        {
            var result = TimeSpan.Zero;

            var shift = (int)CurrentShift + 1;

            for (int day = _currentDayIndex; day < ScheduleState.GetLength(0); day++)
            {
                for (; shift < ScheduleState.GetLength(1); shift++)
                {
                    result += GeneralConstants.RegularShiftLenght;
                }
                shift = 0;
            }

            return result;
        }

        private TimeSpan CalculateNurseHoursToNextShift(int nurseId)
        {
            var hoursToNextShift = TimeSpan.Zero;

            var shift = (int)CurrentShift + 1;

            for (int day = _currentDayIndex; day < ScheduleState.GetLength(0); day++)
            {
                for (; shift < ScheduleState.GetLength(1); shift++)
                {
                    if (ScheduleState[day, shift] != null && ScheduleState[day, shift].Contains(nurseId))
                    {
                        break;
                    }
                    if (shift == (int)ShiftIndex.Day && MorningShiftsState[day] != null && MorningShiftsState[day]
                        .Contains(nurseId))
                    {
                        break;
                    }
                    hoursToNextShift += GeneralConstants.RegularShiftLenght;
                }
                shift = 0;
            }

            return hoursToNextShift;
        }

        private void AddNurseToRegularShift(int nurseId)
        {
            AddNurseToRegularShift(nurseId, CurrentDay, CurrentShift);
        }

        private void AddNurseToMorningShift(int nurseId)
        {
            AddNurseToMorningShift(nurseId, CurrentDay);
        }

        private void AddNurseToRegularShift(int nurseId, int dayNumber, ShiftIndex shift)
        {
            if (ScheduleState[dayNumber - 1, (int)shift] == null)
            {
                ScheduleState[dayNumber - 1, (int)shift] = new HashSet<int>
                {
                    nurseId,
                };
            }
            else
            {
                ScheduleState[dayNumber - 1, (int)shift].Add(nurseId);
            }
        }

        private void AddNurseToMorningShift(int nurseId, int dayNumber)
        {
            if (MorningShiftsState[dayNumber - 1] == null)
            {
                MorningShiftsState[dayNumber - 1] = new HashSet<int>
                {
                    nurseId,
                };
            }
            else
            {
                MorningShiftsState[dayNumber -1].Add(nurseId);
            }
        }
    }
}
