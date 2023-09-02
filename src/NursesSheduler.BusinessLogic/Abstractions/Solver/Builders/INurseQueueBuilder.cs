using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.Enums;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Builders
{
    internal interface INurseQueueBuilder : IBuilder<Queue<int>>
    {
        void InitializeBuilder(IEnumerable<INurseState> nurses);
        INurseQueueBuilder ProritiseWorkersOnTimeOff(int currentDay);
        INurseQueueBuilder FilterTeam(NurseTeams nursesTeam);
        INurseQueueBuilder OrderByLongestBreak();
        INurseQueueBuilder OrderByLowestNumberOfHolidayShitfs();
        INurseQueueBuilder OrderByLowestNumberOfNightShitfs();
        INurseQueueBuilder ProritisePreviousDayShiftWorkers(HashSet<int> previousShift);
    }
}