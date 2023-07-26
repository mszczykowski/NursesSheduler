using NursesScheduler.BusinessLogic.Abstractions.Services;
using NursesScheduler.BusinessLogic.Abstractions.Solver.States;
using NursesScheduler.Domain.Entities;
using NursesScheduler.Domain.ValueObjects.Stats;

namespace NursesScheduler.BusinessLogic.Abstractions.Solver.Directors
{
    internal interface INursesStatesDirector
    {
        IEnumerable<INurseState> BuildNurseStats(Schedule currentSchedule, QuarterStats quarterStats, ScheduleStats previousScheduleStats, ScheduleStats currentScheduleStats, ScheduleStats nextScheduleStats, IWorkTimeService workTimeService, IEnumerable<Nurse> nurses);
    }
}