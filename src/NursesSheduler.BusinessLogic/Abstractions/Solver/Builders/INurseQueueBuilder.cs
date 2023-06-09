namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Builders
{
    internal interface INurseQueueBuilder
    {
        Queue<int> GetResult();
        INurseQueueBuilder OrderByLongestBreak();
        INurseQueueBuilder OrderByLowestNumberOfHolidayShitfs();
        INurseQueueBuilder OrderByLowestNumberOfNightShitfs();
        INurseQueueBuilder ProritisePreviousDayShiftWorkers(List<int> previousShift);
        INurseQueueBuilder RemoveEmployeesOnPTO(int dayNumber);
    }
}