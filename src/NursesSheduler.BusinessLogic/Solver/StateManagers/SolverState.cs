using NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers;
using NursesScheduler.BusinessLogic.Solver.Enums;
using NursesScheduler.Domain;
using NursesScheduler.Domain.Entities;

namespace NursesScheduler.BusinessLogic.Solver.StateManagers
{
    internal sealed class SolverState : ISolverState
    {
        public int CurrentDay { get; set; }
        public int CurrentDayIndex => CurrentDay - 1;
        public int DayNumberInQuarter { get; set; }
        public int WeekInQuarter => (int)Math.Ceiling((decimal)DayNumberInQuarter / 7) - 1;
        public ShiftIndex CurrentShift { get; set; }
        public int EmployeesToAssignForCurrentShift { get; set; }
        public int ShortShiftsToAssign { get; set; }
        public List<INurseState> Nurses { get; }
        public List<int>[,] ScheduleState { get; }
        public List<int>[] MorningShiftsState { get; }
        public SolverState(List<INurseState> nurses, int numberOfDays, int numberOfShifts)
        {
            CurrentDay = 1;
            CurrentShift = 0;
            EmployeesToAssignForCurrentShift = 0;
            Nurses = new List<INurseState>();
            nurses.ForEach(nurse =>
            {
                Nurses.Add(new NurseState(nurse));
            });
            ScheduleState = new List<int>[numberOfDays, numberOfShifts];
            MorningShiftsState = new List<int>[numberOfDays];
        }

        public SolverState(ISolverState stateToCopy)
        {
            CurrentDay = stateToCopy.CurrentDay;

            CurrentShift = stateToCopy.CurrentShift;

            EmployeesToAssignForCurrentShift = stateToCopy.EmployeesToAssignForCurrentShift;

            Nurses = new List<INurseState>();

            stateToCopy.Nurses.ForEach(nurse =>
            {
                Nurses.Add(new NurseState(nurse));
            });

            ScheduleState = new List<int>[stateToCopy.ScheduleState.GetLength(0),
                stateToCopy.ScheduleState.GetLength(1)];

            for (int i = 0; i < ScheduleState.GetLength(0); i++)
            {
                for (int j = 0; j < ScheduleState.GetLength(1); j++)
                {
                    if (stateToCopy.ScheduleState[i, j] != null)
                    {
                        ScheduleState[i, j] = new List<int>(stateToCopy.ScheduleState[i, j]);
                    }
                }
            }

            MorningShiftsState = new List<int>[stateToCopy.MorningShiftsState.Length];

            for (int i = 0; i < MorningShiftsState.Length; i++)
            {
                if (stateToCopy.MorningShiftsState[i] != null)
                {
                    MorningShiftsState[i] = new List<int>(stateToCopy.MorningShiftsState[i]);
                }
            }

            ShortShiftsToAssign = stateToCopy.ShortShiftsToAssign;
        }

        public void AdvanceState()
        {
            if (EmployeesToAssignForCurrentShift > 0)
            {
                EmployeesToAssignForCurrentShift--;
            }
            else
            {
                ShortShiftsToAssign--;
            }

            if (EmployeesToAssignForCurrentShift <= 0 && ShortShiftsToAssign <= 0)
            {
                CurrentShift++;
                if ((int)CurrentShift >= GeneralConstants.NumberOfShifts)
                {
                    CurrentShift = 0;
                    CurrentDay++;
                    DayNumberInQuarter++;

                    if (CurrentDay - 1 < ScheduleState.GetLength(0))
                    {
                        foreach (var e in Nurses)
                        {
                            e.AdvanceState(CalculateNurseHoursToNextShift(e.NurseId));
                        }
                    }
                }
            }
        }

        public List<int> GetPreviousDayShift()
        {
            if (CurrentDay == 1)
            {
                return new List<int>();
            }

            List<int> result;

            if (CurrentShift == ShiftIndex.Day)
            {
                result = ScheduleState[CurrentDayIndex - 1, (int)ShiftIndex.Day];
                if (MorningShiftsState[CurrentDayIndex - 1] != null)
                {
                    result.AddRange(MorningShiftsState[CurrentDayIndex - 1]);
                }
            }
            else
            {
                result = ScheduleState[CurrentDayIndex, (int)ShiftIndex.Day];
                if (MorningShiftsState[CurrentDayIndex] != null)
                {
                    result.AddRange(MorningShiftsState[CurrentDayIndex]);
                }
            }

            return result;
        }

        public void AssignNurseToRegularShift(INurseState nurse, bool isHoliday,
            DepartamentSettings departamentSettings)
        {
            if (ScheduleState[CurrentDayIndex, (int)CurrentShift] == null)
            {
                ScheduleState[CurrentDayIndex, (int)CurrentShift] = new List<int>
                {
                    nurse.NurseId
                };
            }
            else
            {
                ScheduleState[CurrentDayIndex, (int)CurrentShift].Add(nurse.NurseId);
            }

            nurse.UpdateStateOnRegularShiftAssign(isHoliday, CurrentShift, WeekInQuarter, departamentSettings,
                CalculateNurseHoursToNextShift(nurse.NurseId));
        }

        public void AssignEmployeeToMorningShift(INurseState nurse, MorningShift morningShift)
        {
            if (MorningShiftsState[CurrentDayIndex] == null)
            {
                MorningShiftsState[CurrentDayIndex] = new List<int> { nurse.NurseId };
            }
            else
            {
                MorningShiftsState[CurrentDayIndex].Add(nurse.NurseId);
            }

            nurse.UpdateStateOnMorningShiftAssign(morningShift, WeekInQuarter,
                CalculateNurseHoursToNextShift(nurse.NurseId));
        }

        public List<int> GetPreviousShift()
        {
            int previousShift = CurrentShift - 1 < 0 ? 1 : 0;
            int previousDay = previousShift == 0 ? CurrentDay : CurrentDay - 1;
            if (previousDay - 1 < 0) return null;
            return ScheduleState[previousDay - 1, previousShift];
        }
        public List<int> GetNextShift()
        {
            int nextShift = (int)CurrentShift + 1 > 1 ? 0 : 1;
            int nextDay = nextShift == 1 ? CurrentDay : CurrentDay + 1;
            if (nextDay - 1 >= ScheduleState.GetLength(0)) return null;
            return ScheduleState[nextDay - 1, nextShift];
        }

        public TimeSpan GetHoursToScheduleEnd()
        {
            var result = TimeSpan.Zero;

            var shift = (int)CurrentShift + 1;

            for (int day = CurrentDayIndex; day < ScheduleState.GetLength(0); day++)
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

            for (int day = CurrentDayIndex; day < ScheduleState.GetLength(0); day++)
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
    }
}
