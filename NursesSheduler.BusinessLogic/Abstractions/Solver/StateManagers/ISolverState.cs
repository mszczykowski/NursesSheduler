namespace NursesScheduler.BusinessLogic.Abstractions.Solver.StateManagers
{
    internal interface ISolverState
    {
        int CurrentDay { get; set; }
        int DayNumberInQuarter { get; set; }
        int CurrentShift { get; set; }
        int EmployeesToAssignForCurrentShift { get; set; }
        int ShortShiftsToAssign { get; set; }
        List<INurseState> Nurses { get; }
        List<int>[,] ScheduleState { get; }
        void AdvanceState();
        void AssignEmployee(INurseState employee, bool isHoliday, int weekInQuarter);
        void AssignEmployeeToShortShift(INurseState employee, TimeSpan shortShiftLenght, int weekInQuarter);
        List<int> GetPreviousDayShift();
        List<int> GetPreviousShift();
        List<int> GetNextShift();
        List<int>[] AssignedShortShifts { get; }
    }
}