using SheduleSolver.Domain.Enums;
using SheduleSolver.Domain.Models;
using SheduleSolver.Domain.Models.Calendar;
using SolverService.Interfaces.Services;
using SolverService.Interfaces.StateManagers;

namespace SolverService.Implementation.Services
{
    internal sealed class WorkTimeService : IWorkTimeService
    {
        private readonly Quarter _currentQuarter;
        private readonly Month _currentMonth;
        private readonly WorkTimeConfiguration _workTimeConfiguration;

        private int[,] shiftCapacities;
        private int minimalNumberOfWorkersOnShift;
        private int[,] shortShiftsCapacities;


        public WorkTimeService(Quarter quarter, int currentMonthIndex, WorkTimeConfiguration workTimeConfiguration)
        {
            _currentQuarter = quarter;
            _currentMonth = _currentQuarter.Months[currentMonthIndex];
            _workTimeConfiguration = workTimeConfiguration;
        }

        public bool IsHoliday(Day day)
        {
            return day.IsHoliday == true || day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday;
        }

        public int GetNumberOfWorkingDays(Month month)
        {
            int workingDays = 0;
            foreach (var day in month.Days)
            {
                if (!IsHoliday(day)) workingDays++;
                if (day.IsHoliday == true && day.DayOfWeek == DayOfWeek.Saturday)
                    workingDays--;
            }
            return workingDays;
        }

        private TimeSpan GetWorkingTimePerNurse(Month month)
        {
            return GetNumberOfWorkingDays(month) * _workTimeConfiguration.WorkTimePerDay;
        }


        public void InitialiseEmployeeTimeToAssign(List<IEmployeeState> employees)
        {
            var workingTimePerNurse = GetWorkingTimePerNurse(_currentMonth);
            foreach (var employee in employees)
            {
                employee.WorkTimeToAssign += workingTimePerNurse;

                int PTORegularDays = 0;

                for (int i = 0; i < employee.PTO.Length; i++)
                {
                    if (employee.PTO[i] == true && !IsHoliday(_currentMonth.Days[i])) PTORegularDays++;
                }

                employee.PTOTimeToAssign = Math.Floor(PTORegularDays * _workTimeConfiguration.WorkTimePerDay /
                    _workTimeConfiguration.ShiftLenght) * _workTimeConfiguration.ShiftLenght;

                employee.WorkTimeToAssign -= employee.PTOTimeToAssign;

                employee.NumberOfShiftsToAssign =
                    (int)Math.Floor(employee.WorkTimeToAssign / _workTimeConfiguration.ShiftLenght);

                int numberOfDays = 0;

                foreach (var m in _currentQuarter.Months)
                {
                    numberOfDays += m.Days.Length;
                }

                int numberOfWeeksInQuarter = (int)Math.Ceiling((decimal)numberOfDays / 7);

                employee.WorkTimeAssignedInWeek = new TimeSpan[numberOfWeeksInQuarter];
            }
        }

        public void InitialiseShiftCapacities(List<IEmployeeState> employees, Random random)
        {
            var totalNumberOfShiftsToAssign = GetTotalNumberOfShiftsToAssign(employees);

            minimalNumberOfWorkersOnShift = GetMinimalNumberOfNursesOnShift(totalNumberOfShiftsToAssign);

            var numberOfSurplusShifts = GetNumberOfSurplusShifts(totalNumberOfShiftsToAssign, minimalNumberOfWorkersOnShift);

            var numberOfWorkingDays = GetNumberOfWorkingDays(_currentMonth);

            var regularDayShiftCapacities = new int[numberOfWorkingDays];
            var nightShiftsCapacities = new int[_currentMonth.Days.Count()];
            var holidayDayShiftCapacities = new int[_currentMonth.Days.Count() - numberOfWorkingDays];

            Array.Fill(regularDayShiftCapacities, minimalNumberOfWorkersOnShift);
            Array.Fill(nightShiftsCapacities, minimalNumberOfWorkersOnShift);
            Array.Fill(holidayDayShiftCapacities, minimalNumberOfWorkersOnShift);

            if (minimalNumberOfWorkersOnShift < _workTimeConfiguration.TargetNumberOfNursesOnShift)
            {
                if (numberOfSurplusShifts > 0)
                {
                    for (int i = 0; i < regularDayShiftCapacities.Length; i++)
                    {
                        regularDayShiftCapacities[i]++;
                        numberOfSurplusShifts--;
                        if (numberOfSurplusShifts == 0) break;
                    }
                }

                if (numberOfSurplusShifts > 0)
                {
                    for (int i = 0; i < holidayDayShiftCapacities.Length; i++)
                    {
                        regularDayShiftCapacities[i]++;
                        numberOfSurplusShifts--;
                        if (numberOfSurplusShifts == 0) break;
                    }
                }

                if (numberOfSurplusShifts > 0)
                {
                    for (int i = 0; i < nightShiftsCapacities.Length; i++)
                    {
                        regularDayShiftCapacities[i]++;
                        numberOfSurplusShifts--;
                        if (numberOfSurplusShifts == 0) break;
                    }
                }
            }
            else
            {
                int arrayIterator = 0;
                while (numberOfSurplusShifts > 0)
                {
                    regularDayShiftCapacities[arrayIterator++]++;
                    numberOfSurplusShifts--;
                    if (arrayIterator >= regularDayShiftCapacities.Length) arrayIterator = 0;
                }
            }

            regularDayShiftCapacities = regularDayShiftCapacities.OrderBy(i => random.Next()).ToArray();
            holidayDayShiftCapacities = holidayDayShiftCapacities.OrderBy(i => random.Next()).ToArray();
            nightShiftsCapacities = nightShiftsCapacities.OrderBy(i => random.Next()).ToArray();

            int regularShiftIterator = 0;
            int nightShiftIterator = 0;
            int holidayShiftIterator = 0;

            List<int> regularDays = new List<int>();

            shiftCapacities = new int[_currentMonth.Days.Length, 2];
            for (int i = 0; i < _currentMonth.Days.Length; i++)
            {
                shiftCapacities[i, (int)ShiftType.night] = nightShiftsCapacities[nightShiftIterator++];

                if (IsHoliday(_currentMonth.Days[i]))
                    shiftCapacities[i, (int)ShiftType.day] = holidayDayShiftCapacities[holidayShiftIterator++];

                else
                {
                    shiftCapacities[i, (int)ShiftType.day] = regularDayShiftCapacities[regularShiftIterator++];
                    regularDays.Add(i);
                }
            }
            shortShiftsCapacities = new int[_currentMonth.Days.Length, 2];

            regularDays.OrderBy(i => shiftCapacities[i, (int)ShiftType.day]);

            int[,] shortShifts = new int[regularDays.Count < employees.Count ? employees.Count : regularDays.Count, 2];

            for (int i = 0; i < shortShifts.GetLength(0); i++)
            {
                if (i > shortShifts.GetLength(0)) break;
                shortShifts[i, 0] = regularDays[i];
            }
            for (int i = 0; i < employees.Count; i++)
            {
                shortShifts[i % shortShifts.Length, 1]++;
            }

            for (int i = 0; i < shortShifts.GetLength(0); i++)
            {
                shortShiftsCapacities[shortShifts[i, 0], (int)ShiftType.day] = shortShifts[i, 1];
            }
        }

        public int GetNumberOfNursesForShift(ShiftType shiftType, int dayNumber)
        {
            return shiftCapacities[dayNumber - 1, (int)shiftType];
        }




        private int GetTotalNumberOfShiftsToAssign(List<IEmployeeState> employees)
        {
            int totalNumberOfShifts = 0;
            foreach (var employee in employees)
            {
                totalNumberOfShifts += (int)Math.Floor(employee.WorkTimeToAssign / _workTimeConfiguration.ShiftLenght);
            }
            return totalNumberOfShifts;
        }

        private int GetMinimalNumberOfNursesOnShift(int totalNumberOfShiftsToAssign)
        {
            int calculatedMinimal = totalNumberOfShiftsToAssign / (_currentMonth.Days.Length * _workTimeConfiguration.ShiftDetails.Count);

            return _workTimeConfiguration.TargetNumberOfNursesOnShift < calculatedMinimal ?
                _workTimeConfiguration.TargetNumberOfNursesOnShift : calculatedMinimal;
        }

        private int GetNumberOfSurplusShifts(int totalNumberOfShiftsToAssign, int minimalNumberOfNursesOnShift)
        {
            return totalNumberOfShiftsToAssign - minimalNumberOfNursesOnShift * _currentMonth.Days.Length * 2;
        }

        public void InitialiseShortShifts()
        {
            List<TimeSpan> result = new List<TimeSpan>();
            TimeSpan surplusTime = TimeSpan.Zero;

            foreach (var month in _currentQuarter.Months)
            {
                var workTimeInMonth = GetWorkingTimePerNurse(month);
                var x = GetWorkingTimePerNurse(month);
                var y = (int)Math.Floor(workTimeInMonth / _workTimeConfiguration.ShiftLenght);

                surplusTime += workTimeInMonth - (int)Math.Floor(workTimeInMonth / _workTimeConfiguration.ShiftLenght)
                    * _workTimeConfiguration.ShiftLenght;
            }

            while (surplusTime > _workTimeConfiguration.ShiftLenght
                && surplusTime - _workTimeConfiguration.ShiftLenght > _workTimeConfiguration.TargetMinimalShiftLenght)
            {
                surplusTime -= _workTimeConfiguration.ShiftLenght;
                result.Add(_workTimeConfiguration.ShiftLenght);
            }

            if (surplusTime > _workTimeConfiguration.ShiftLenght)
            {
                result.Add(surplusTime / 2);
                result.Add(surplusTime / 2);
            }
            else result.Add(surplusTime);

            _currentQuarter.SurplusShifts = result;
        }

        public int GetNumberOfShortShifts(ShiftType shiftType, int dayNumber)
        {
            return shortShiftsCapacities[dayNumber - 1, (int)shiftType];
        }
    }
}
