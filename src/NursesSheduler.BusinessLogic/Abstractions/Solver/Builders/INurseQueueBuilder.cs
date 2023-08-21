using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Builders
{
    internal interface INurseQueueBuilder : IBuilder<Queue<int>>
    {
        INurseQueueBuilder ProritiseWorkersOnTimeOff(int currentDay);
        INurseQueueBuilder FilterTeam(NurseTeams nursesTeam);
        INurseQueueBuilder OrderByLongestBreak();
        INurseQueueBuilder OrderByLowestNumberOfHolidayShitfs();
        INurseQueueBuilder OrderByLowestNumberOfNightShitfs();
        INurseQueueBuilder ProritisePreviousDayShiftWorkers(HashSet<int> previousShift);
    }
}