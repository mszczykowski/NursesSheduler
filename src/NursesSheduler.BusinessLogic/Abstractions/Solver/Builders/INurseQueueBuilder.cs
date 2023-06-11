namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Builders
{
    internal interface INurseQueueBuilder
    {
        Queue<int> GetResult();
        INurseQueueBuilder OrderByLongestBreak();
        INurseQueueBuilder OrderByLowestNumberOfHolidayShitfs();
        INurseQueueBuilder OrderByLowestNumberOfNightShitfs();
        INurseQueueBuilder ProritisePreviousDayShiftWorkers(HashSet<int> previousShift);
        INurseQueueBuilder RemoveEmployeesOnPTO(int dayNumber);
    }
}